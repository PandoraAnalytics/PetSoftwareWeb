using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace BABusiness
{
    public class Ticket : BusinessBase
    {
        public enum Status
        {
            NONE = 0,
            NEW = 1,
            DEFERRED = 2,
            APPROVEDFORDEVELOPMENT = 3,
            TESTING = 4,
            DEVELOPMENT = 5,
            APPROVED = 6,
            ROLLEDOUT = 7,
            CLOSED = 8,
            ESTIMATEEDT = 10,
            WAITINGFORDEVAPPROVAL = 11,
            UPDATE = 12,
            COMMENT = 13,
            UPDATECOMMENT = 14,
            ADDDOCUMENT = 15,
            UPDATEDOCUMENT = 16,
            EDTCHANGE = 17,
            APPROVEDFORDEVELOPMENTVONEEDED = 18,
            APPROVEROLLEDOUT = 19,
            VONEEDEDUPDATED = 20,
            INDIVIDUALUSERVOAPPROVED = 101,
            ADDOPTIONALEMAILS = 102,
        }

        public static string GetStatusLogText(object xiStatus, object xiAdditionMessage)
        {
            string statusText = string.Empty;
            try
            {
                switch (Convert.ToInt32(xiStatus))
                {
                    case (int)Status.NEW:
                        statusText = "The new ticket has been created";
                        break;

                    case (int)Status.UPDATE:
                        statusText = "The ticket has been updated.";
                        break;

                    case (int)Status.ADDDOCUMENT:
                        statusText = "New document has been uploaded";
                        break;

                    case (int)Status.UPDATECOMMENT:
                        statusText = "A comment has been updated";
                        break;

                    case (int)Status.COMMENT:
                        statusText = "New comment has been added";
                        break;

                    case (int)Status.UPDATEDOCUMENT:
                        statusText = "A document has been updated";
                        break;

                    case (int)Status.DEFERRED:
                        statusText = "The ticket has been deffered";
                        break;

                    case (int)Status.APPROVEDFORDEVELOPMENT:
                        statusText = "The ticket has been approved for development";
                        if (xiAdditionMessage != DBNull.Value) statusText = statusText + ", " + xiAdditionMessage;
                        break;

                    case (int)Status.TESTING:
                        statusText = "The ticket has been marked as testing";
                        if (xiAdditionMessage != DBNull.Value) statusText = statusText + ", " + xiAdditionMessage;
                        break;

                    case (int)Status.DEVELOPMENT:
                        statusText = "The ticket has been into development";
                        if (xiAdditionMessage != DBNull.Value) statusText = statusText + ", " + xiAdditionMessage;
                        break;

                    case (int)Status.APPROVED:
                        statusText = "The ticket has been approved";
                        break;

                    case (int)Status.ROLLEDOUT:
                        statusText = "The ticket has been rolledout";
                        break;

                    case (int)Status.CLOSED:
                        statusText = "The ticket has been closed";
                        if (xiAdditionMessage != DBNull.Value) statusText = statusText + ", " + xiAdditionMessage;
                        break;

                    case (int)Status.ESTIMATEEDT:
                        statusText = "EDT has been estimated for the ticket";
                        break;

                    case (int)Status.WAITINGFORDEVAPPROVAL:
                        statusText = "The ticket has been waiting for development approval";
                        if (xiAdditionMessage != DBNull.Value) statusText = statusText + ", " + xiAdditionMessage;
                        break;

                    case (int)Status.EDTCHANGE:
                        statusText = "EDT has been changed";
                        if (xiAdditionMessage != DBNull.Value) statusText = statusText + " (old: " + xiAdditionMessage + " hrs)";
                        break;

                    case (int)Status.VONEEDEDUPDATED:
                        statusText = "A VO Needed has been updated";
                        if (xiAdditionMessage != DBNull.Value) statusText = statusText + " - " + xiAdditionMessage;
                        break;

                    case (int)Status.APPROVEROLLEDOUT:
                        statusText = "The ticket has been approved rollout";
                        break;

                    case (int)Status.INDIVIDUALUSERVOAPPROVED:
                        statusText = "The ticket has been approved for development";
                        if (xiAdditionMessage != DBNull.Value) statusText = statusText + " (L" + xiAdditionMessage + ")";
                        break;

                    default:
                        statusText = "-";
                        break;
                }
            }
            catch { }

            return statusText;
        }


        #region Tickets Priority

        public bool AddPriority(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into ticket_priority(priorityname)values(@priorityname)";

            Parameter param1 = new Parameter("priorityname", xiCollection["priorityname"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdatePriority(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_priority set priorityname=@priorityname where active = 1 and id=@id";

            Parameter param1 = new Parameter("priorityname", xiCollection["priorityname"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetPriorityDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select id,priorityname from ticket_priority where active = 1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("priorityname", Convert.ToString(objdb.GetValue(reader1, "priorityname")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetPriorityDetail", x.Message);
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

        public static int GetTicketPriorityCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(tp.id) as maxcount from ticket_priority tp where tp.active = 1";
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
                objdb.Write_log_file("GetTicketPriorityCount", x.Message);
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

        public static DataSet GetTicketPriorityDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = "select tp.id, tp.priorityname from ticket_priority tp where tp.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by tp.id desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }


        public static bool DeleteTicketPriority(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update ticket_priority set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetAllPriority()
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select id,priorityname from ticket_priority where active = 1 order by priorityname";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dataTable;
        }

        #endregion

        //new 21 jun 2024 nilesh

        #region Application

        public bool AddTicketApplication(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into ticket_applications ([name]) values(@name)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();
            return (value > 0);
        }

        public bool UpdateTicketApplication(NameValueCollection xiCollection, object xiApplicationId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_applications set [name]=@name where id=@id";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiApplicationId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetTicketApplication(object xiApplicationId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            string query = "select * from ticket_applications where id = " + Utils.ConvertToDBString(xiApplicationId, Utils.DataType.Integer) + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                while (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));

                    break;
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetTicketApplication", x.Message);
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

        public static int GetTicketApplicationCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(id) as maxcount from ticket_applications where active = 1";
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
                objdb.Write_log_file("GetTicketApplicationCount", x.Message);
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

        public static DataSet GetTicketApplicationDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select id, [name] from ticket_applications where active=1 ";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by [name] offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);

            objdb.Disconnectdb();
            return ds;
        }

        public static string DeleteTicketApplication(int xiApplicationId)
        {
            string query = string.Format("update ticket_applications set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiApplicationId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0) ? "Record deleted." : "false";
        }

        public static string SearchTicketApplication(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("([name] like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            return builder.ToString();
        }

        public static DataTable GetApplicationsForTicket()
        {
            string query = "select id, name from ticket_applications where active = 1 order by name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        #endregion

        public static DataSet GetStatuses()
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from ticket_status order by name";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static DataTable GetUserStatuses(string xiUserId)
        {
            string query = "select * from ticket_users where [user_id] = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable ds = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }
        public bool UpdateStatus(NameValueCollection xiCollection, object xiTicketId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_tickets set status=@status,lastmodified=getutcdate(),updatedby=@userid where id = @ticketid and active=1";

            Parameter param1 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param2 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param3 = new Parameter("ticketid", xiTicketId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();
            return (value > 0);
        }

        public bool AddTicketComment(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into ticket_comments(ticket_id, created, createdby, comment) values(";
            query += xiCollection["ticketid"] + ", ";
            query += "getutcdate(), ";
            query += xiCollection["userid"] + ", ";
            query += Utils.ConvertToDBString(xiCollection["comment"], Utils.DataType.String);
            query += ")";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateComment(NameValueCollection xiCollection, object xiCommentId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_comments set ";
            query += "comment = " + Utils.ConvertToDBString(xiCollection["comment"], Utils.DataType.String);
            query += " where ticket_id = " + xiCollection["ticketid"] + " and id = " + Utils.ConvertToDBString(xiCommentId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetTicketCommentsCount(string xiTicketId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(sc.id) as maxcount from ticket_comments sc inner join [user] u on u.id = sc.createdby where sc.ticket_id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer);
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
                objdb.Write_log_file("ticket_GetCommentsCount", x.Message);
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

        public static DataSet GetTicketComments(int xiPage, string xiTicketId)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select sc.*, u.fname as user_pre_name, u.lname as user_family_name from ticket_comments sc inner join [user] u on u.id = sc.createdby where sc.ticket_id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer) + " order by sc.created desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("author", typeof(string));
                ds.Tables[0].Columns.Add("createddate", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["author"] = row["user_pre_name"] + " " + row["user_family_name"];

                    DateTime tempDate = Convert.ToDateTime(row["created"]);
                    if (tempDate != DateTime.MinValue)
                    {
                        row["createddate"] = tempDate.ToString(BusinessBase.DateTimeFormat);
                    }
                }
            }

            return ds;
        }

        public static DataSet GetComment(int xiCommentId)
        {
            string query = "select id, comment from ticket_comments where id = " + Utils.ConvertToDBString(xiCommentId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public bool AddDocument(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into ticket_documents(ticket_id, filename, created, createdby, comment) values(";
            query += xiCollection["ticketid"] + ", ";
            query += Utils.ConvertToDBString(xiCollection["filename"], Utils.DataType.String) + ", ";
            query += "getutcdate(), ";
            query += xiCollection["userid"] + ", ";
            query += Utils.ConvertToDBString(xiCollection["comment"], Utils.DataType.String, true);
            query += ")";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateDocument(NameValueCollection xiCollection, object xiDocumentId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_documents set ";
            query += "comment = " + Utils.ConvertToDBString(xiCollection["comment"], Utils.DataType.String, true);
            query += " where ticket_id = " + xiCollection["ticketid"] + " and id = " + Utils.ConvertToDBString(xiDocumentId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateETD(NameValueCollection xiCollection, object xiTicketId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_tickets set ";
            query += "etd = " + Utils.ConvertToDBString(xiCollection["etd"], Utils.DataType.Integer) + ", ";
            query += "updatedby = " + Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer) + ", ";
            query += "lastmodified = getutcdate()";
            query += " where id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer) + " and active=1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateCompletionDate(NameValueCollection xiCollection, object xiTicketId)
        {
            string query = "update ticket_tickets set ";
            query += "completion_date = " + Utils.ConvertToDBString(xiCollection["completiondate"], Utils.DataType.Date) + ", ";
            query += "updatedby = " + Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer) + ", ";
            query += "lastmodified = getutcdate()";
            query += " where id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer) + " and active=1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataSet GetTicketDocuments(int xiPage, string xiticketId)
        {
            if (string.IsNullOrEmpty(xiticketId)) return null;

            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select sd.*, u.fname as user_pre_name, u.lname as user_family_name from ticket_documents sd inner join [user] u on u.id = sd.createdby where sd.ticket_id = " + Utils.ConvertToDBString(xiticketId, Utils.DataType.Integer) + " order by sd.created desc";

            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("author", typeof(string));
                ds.Tables[0].Columns.Add("createddate", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["author"] = row["user_pre_name"] + " " + row["user_family_name"];

                    DateTime tempDate = Convert.ToDateTime(row["created"]);
                    if (tempDate != DateTime.MinValue)
                    {
                        row["createddate"] = tempDate.ToString(BusinessBase.DateTimeFormat);
                    }
                }
            }

            return ds;
        }

        public static DataSet GetDocument(int xiDocumentId)
        {
            string query = "select id, comment from ticket_documents where id = " + Utils.ConvertToDBString(xiDocumentId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static bool DeleteTicketComment(int xiId, int xiLoggedId)
        {
            if (xiLoggedId <= 0) return false;

            string query = string.Format("delete from ticket_comments where id = {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool DeleteTicketDocument(int xiId, int xiLoggedId)
        {
            if (xiLoggedId <= 0) return false;

            string query = string.Format("delete from ticket_documents where id = {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }
       
        #region Ticket Users

        public bool Add(string[] xiStatus, object xiUserId)
        {
            if (xiStatus == null || xiStatus.Length == 0) return false;

            string query = "delete from ticket_users where user_id = {0};";
            string insertquery = "insert into ticket_users(user_id, status) values({0}, {1});";
            foreach (string status in xiStatus)
            {
                if (string.IsNullOrEmpty(status)) continue;
                query += string.Format(insertquery, "{0}", status);
            }
            query = string.Format(query, xiUserId);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string UserSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;
            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                iswhere = true;
                builder.Append(string.Format("((u.fname + ' ' + u.lname) like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            //if (xiCollection["folder"] != null && xiCollection["folder"].Length > 0)
            //{
            //    if (iswhere) builder.Append(" and ");
            //    iswhere = true;
            //    builder.Append(string.Format(" a.foldername = {0} ", Utils.ConvertToDBString(xiCollection["folder"], Utils.DataType.String)));
            //}

            return builder.ToString();
        }
        public static NameValueCollection GetUser(object xiUserId) //working
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select u.*,su.status from [user] u left join ticket_users su on su.user_id = u.id where u.active = 1 and u.id =" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and u.isowner = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                while (reader1.Read())
                {
                    if (collection == null)
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

                        collection.Add("countryid", Convert.ToString(objdb.GetValue(reader1, "countryid")));
                        collection.Add("city", Convert.ToString(objdb.GetValue(reader1, "city")));
                        collection.Add("pincode", Convert.ToString(objdb.GetValue(reader1, "pincode")));
                        collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    }
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("ticket_GetUser", x.Message);
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

        public static int GetAllUsersCount(string xiFilter)   // working
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            string query = "select count(u.id) as maxcount from [user] u where u.isowner = 1 and u.active = 1";

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
                objdb.Write_log_file("TicketUser_GetAllUsersCount", x.Message);
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
        public static DataSet GetAllUsers(int xiPage, string xiFilter)  //working
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select u.id as primaryuserid, u.fname as fname, u.lname as lname, u.email as email, u.phone as phone,u.type,u.submitdate,u.isowner from [user] u where u.isowner = 1 and u.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by u.fname,u.lname offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only;";

            query += "select tu.user_id, s.name from ticket_users tu inner join ticket_status s on tu.status = s.id";

            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 1)
            {
                ds.Tables[0].Columns.Add("status", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string userid = Convert.ToString(row["primaryuserid"]);

                    DataRow[] foundRows1 = ds.Tables[1].Select("user_id = " + userid);
                    if (foundRows1 == null || foundRows1.Length == 0) continue;

                    string status = string.Empty;
                    foreach (DataRow statusRow in foundRows1)
                    {
                        if (status.Length > 0) status += "; ";
                        status += Convert.ToString(statusRow["name"]);
                    }

                    row["status"] = status;
                    status = "";
                }
            }

            return ds;
        }
        #endregion
      
        #region New Ticket
        public int Add(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;
            if (string.IsNullOrEmpty(xiCollection["optionalemails"])) xiCollection["optionalemails"] = string.Empty;

            int ticketno = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            string selectquery = "select max(ticketno) as maxticketno from ticket_tickets";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, selectquery);

                while (reader1.Read())
                {
                    ticketno = Convert.ToInt32(objdb.GetValue(reader1, "maxticketno"));
                    break;
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("AddTicket", x.Message);
            }
            finally
            {
                if (reader1 != null)
                {
                    reader1.Close();
                    reader1 = null;
                }
            }
            if (ticketno < 0) ticketno = 0;
            ticketno = ticketno + 1;

            string query = "insert into ticket_tickets(ticketno, header, description, application, isbug, etd, active, created, lastmodified, createdby, updatedby, voneeded,[priority], [status], optionalemails) values(";

            query += ticketno + ", ";
            query += Utils.ConvertToDBString(xiCollection["header"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["description"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["application"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["isbug"], Utils.DataType.Integer) + ", ";
            query += "0, ";
            query += "1, ";
            query += "getutcdate(), ";
            query += "getutcdate(), ";
            query += Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["voneeded"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["priority"], Utils.DataType.Integer) + ", ";
            query += "1" + ", ";
            query += Utils.ConvertToDBString(xiCollection["optionalemails"], Utils.DataType.String);
            query += ")";

            string query2 = "Select Scope_Identity()";

            int ticketId = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2);
            objdb.Disconnectdb();

            return ticketId;
        }

        public bool UpdateTicket(NameValueCollection xiCollection, object xiTicketId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_tickets set ";
            query += "header = " + Utils.ConvertToDBString(xiCollection["header"], Utils.DataType.String) + ", ";
            query += "description = " + Utils.ConvertToDBString(xiCollection["description"], Utils.DataType.String) + ", ";
            query += "application = " + Utils.ConvertToDBString(xiCollection["application"], Utils.DataType.Integer) + ", ";
            query += "isbug = " + Utils.ConvertToDBString(xiCollection["isbug"], Utils.DataType.Integer) + ", ";
            query += "optionalemails = " + Utils.ConvertToDBString(xiCollection["optionalemails"], Utils.DataType.String) + ", ";
            query += "updatedby = " + Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer) + ", ";
            query += "lastmodified = getutcdate()";
            query += " where id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer) + " and active=1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetTicket(object xiTicketId)//no user id for send email purpose
        {
            NameValueCollection collection = null;

            string query = "select t.ticketno, t.header, u1.lname as createdln, u1.fname as createdfn, u1.email as createdemail from ticket_tickets t inner join [user] u1 on u1.id = t.createdby where t.id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();

                    collection.Add("ticketno", Convert.ToString(objdb.GetValue(reader1, "ticketno")));
                    collection.Add("header", Convert.ToString(objdb.GetValue(reader1, "header")));
                    collection.Add("author", Convert.ToString(objdb.GetValue(reader1, "createdfn")) + " " + Convert.ToString(objdb.GetValue(reader1, "createdln")));
                    collection.Add("authoremail", Convert.ToString(objdb.GetValue(reader1, "createdemail")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetTicket_NoUserId", x.Message);
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


        public static NameValueCollection GetTicket(object xiTicketId, object xiUserId)
        {
            NameValueCollection collection = null;

            string query = @"select t.*,p.priorityname, s.[name] as statusname, app.name as appname, u1.isowner,u1.lname as createdln, u1.fname as createdfn, u1.email as createdemail, u2.lname as updatedln, u2.fname as updatedfn,
(select count(*) from ticket_comments tc where tc.ticket_id = t.id) as commentscount,
(select count(*) from ticket_documents tc where tc.ticket_id = t.id) as doccount,
(select top 1 old_entry from ticket_log tl where tl.ticket_id = t.id and tl.message_id = t.status order by tl.datetime desc) as log_comments
from ticket_tickets t inner join [user] u1 on u1.[id] = t.createdby inner join [user] u2 on u2.[id] = t.updatedby 
inner join ticket_applications app on app.id = t.[application] inner join ticket_status s on s.id = t.[status] left join ticket_priority p on p.id = t.priority
where t.active = 1 and t.id = {0}";
            // and (t.createdby = {1} or (t.[status] in (select status from ticket_users where [user_id] = {1}) and t.[application] in (select app_id from system_user_department where [user_id] = {1})))";
            query = string.Format(query, Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer), Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();

                    collection.Add("ticketno", Convert.ToString(objdb.GetValue(reader1, "ticketno")));
                    collection.Add("header", Convert.ToString(objdb.GetValue(reader1, "header")));
                    collection.Add("description", Convert.ToString(objdb.GetValue(reader1, "description")));
                    collection.Add("application", Convert.ToString(objdb.GetValue(reader1, "application")));
                    collection.Add("appname", Convert.ToString(objdb.GetValue(reader1, "appname")));
                    collection.Add("etd", Convert.ToString(objdb.GetValue(reader1, "etd")));
                    collection.Add("isbug", Convert.ToString(objdb.GetValue(reader1, "isbug")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("author", Convert.ToString(objdb.GetValue(reader1, "createdfn")) + " " + Convert.ToString(objdb.GetValue(reader1, "createdln")));
                    collection.Add("authoremail", Convert.ToString(objdb.GetValue(reader1, "createdemail")));
                    collection.Add("createddate", Convert.ToString(objdb.GetValue(reader1, "created")));

                    collection.Add("updatedby", Convert.ToString(objdb.GetValue(reader1, "updatedby")));
                    collection.Add("updatedbyauthor", Convert.ToString(objdb.GetValue(reader1, "updatedfn")) + " " + Convert.ToString(objdb.GetValue(reader1, "updatedln")));
                    collection.Add("updateddate", Convert.ToString(objdb.GetValue(reader1, "lastmodified")));
                    collection.Add("completiondate", Convert.ToString(objdb.GetValue(reader1, "completion_date")));

                    collection.Add("voneeded", Convert.ToString(objdb.GetValue(reader1, "voneeded")));
                    collection.Add("optionalemails", Convert.ToString(objdb.GetValue(reader1, "optionalemails")));
                    collection.Add("priority", Convert.ToString(objdb.GetValue(reader1, "priority")));
                    collection.Add("priorityname", Convert.ToString(objdb.GetValue(reader1, "priorityname")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                    collection.Add("statusname", Convert.ToString(objdb.GetValue(reader1, "statusname")));
                    collection.Add("invoice", Convert.ToString(objdb.GetValue(reader1, "invoice")));
                    collection.Add("commentscount", Convert.ToString(objdb.GetValue(reader1, "commentscount")));
                    collection.Add("doccount", Convert.ToString(objdb.GetValue(reader1, "doccount")));
                    collection.Add("log_comments", Convert.ToString(objdb.GetValue(reader1, "log_comments")));
                    collection.Add("isowner", Convert.ToString(objdb.GetValue(reader1, "isowner")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetTicket", x.Message);
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

        public static int GetTicketsCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(t.id) as maxcount from ticket_tickets t inner join [user] u1 on u1.id = t.createdby inner join [user] u2 on u2.id = t.updatedby inner join ticket_applications app on app.id = t.application inner join ticket_status s on s.id = t.status left join ticket_priority p on p.id = t.priority where t.active = 1 and t.status <> 8";
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
                objdb.Write_log_file("GetTicketsCount", x.Message);
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
        //final working with sort
        public static DataSet GetTickets(int xiPage, string xiFilter, string xiSort)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"select t.id, t.ticketno, t.header, t.isbug, t.etd, t.created, t.[status],t.voneeded, t.[priority], t.createdby, t.created, t.completion_date,p.priorityname,
        t.lastmodified, s.[name] as statusname, app.name as appname, u1.lname as createdln, u1.fname as createdfn, u2.lname as updatedln, 
        u2.fname as updatedfn,
        (select count(*) from ticket_comments tc where tc.ticket_id = t.id) as commentscount,
        (select count(*) from ticket_documents tc where tc.ticket_id = t.id) as doccount,
        (select top 1 old_entry from ticket_log tl where tl.ticket_id = t.id and tl.message_id = t.status order by tl.datetime desc) as log_comments
        from ticket_tickets t inner join [user] u1 on u1.[id] = t.createdby 
        inner join [user] u2 on u2.[id] = t.updatedby 
        inner join ticket_applications app on app.id = t.[application]
        inner join ticket_status s on s.id = t.[status] left join ticket_priority p on p.id = t.priority where t.active = 1 and t.[status] <> 8";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += "  order by " + GetSortOrder(xiSort) + " offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable newTable = Ticket.ProcessTicketTable(ds.Tables[0]);
                ds.Tables.RemoveAt(0);
                ds.Tables.Add(newTable);
            }

            return ds;
        }

        private static string GetSortOrder(string xiSort)
        {
            string sortcolumn = "t.created desc";

            string[] sortorder = BusinessBase.SplitSort(xiSort);
            switch (sortorder[0])
            {
                case "1":
                    sortcolumn = "t.ticketno " + sortorder[1];
                    break;

                case "2":
                    sortcolumn = "t.header " + sortorder[1];
                    break;

                case "3":
                    sortcolumn = "app.name " + sortorder[1];
                    break;

                case "5":
                    sortcolumn = "p.priorityname " + sortorder[1];
                    break;

                case "6":
                    sortcolumn = "s.[name] " + sortorder[1];
                    break;

                case "8":
                    sortcolumn = "t.lastmodified " + sortorder[1];
                    break;

                case "9":
                    sortcolumn = "t.etd " + sortorder[1];
                    break;

                case "10":
                    sortcolumn = "t.voneeded " + sortorder[1];
                    break;

            }

            return sortcolumn;
        }

        private static DataTable ProcessTicketTable(DataTable xiDataTable)
        {
            DataTable dataTable = xiDataTable;
            dataTable.Columns.Add("ticket_no", typeof(string));
            dataTable.Columns.Add("processed_created", typeof(string));
            dataTable.Columns.Add("processed_updatedate", typeof(string));
            dataTable.Columns.Add("processed_completiondate", typeof(string));
            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    if (row["ticketno"] != DBNull.Value) row["ticket_no"] = Convert.ToString(row["ticketno"]).PadLeft(5, '0');

                    if (row["created"] != DBNull.Value)
                    {
                        DateTime tempDate = Convert.ToDateTime(row["created"]);
                        if (tempDate != DateTime.MinValue) row["processed_created"] = tempDate.ToString(BusinessBase.DateTimeFormat);
                    }

                    if (row["lastmodified"] != DBNull.Value)
                    {
                        DateTime tempDate = Convert.ToDateTime(row["lastmodified"]);
                        if (tempDate != DateTime.MinValue) row["processed_updatedate"] = tempDate.ToString(BusinessBase.DateTimeFormat);
                    }

                    if (row["completion_date"] != DBNull.Value)
                    {
                        DateTime tempDate = Convert.ToDateTime(row["completion_date"]);
                        if (tempDate != DateTime.MinValue) row["processed_completiondate"] = tempDate.ToString(BusinessBase.DateFormat);
                    }

                    if (xiDataTable.Columns.Contains("log_comments"))
                    {
                        switch (BusinessBase.ConvertToInteger(row["status"]))
                        {
                            case (int)Ticket.Status.WAITINGFORDEVAPPROVAL:
                                if (BusinessBase.ConvertToInteger(row["voneeded"]) == 1) row["statusname"] = "Waiting for customer approval";
                                break;


                            case (int)Ticket.Status.APPROVEDFORDEVELOPMENT:
                            case (int)Ticket.Status.TESTING:
                                if (row["log_comments"] != DBNull.Value) row["statusname"] += ", " + row["log_comments"];
                                break;
                        }
                    }
                }
                catch { }
            }

            return dataTable;
        }

        public static int GetEDTSum(string xiFilter)
        {
            int totalsum = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select sum(t.etd) as totalsum from ticket_tickets t where t.active = 1 and t.[status] not in (2,7,8)";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalsum = Convert.ToInt32(objdb.GetValue(reader1, "totalsum"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetEDTSum", x.Message);
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

            if (totalsum < 0) totalsum = 0;

            return totalsum;
        }

        public string TicketSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["user"] != null && xiCollection["user"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                iswhere = true; builder.Append(string.Format("(t.createdby = {0} or (t.status in (select status from ticket_users where user_id = {0})))", xiCollection["user"]));
            }

            if (xiCollection["application"] != null && xiCollection["application"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(t.application = {0})", xiCollection["application"]));
            }

            if (BusinessBase.ConvertToInteger(xiCollection["ticketno"]) > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(t.ticketno = {0})", Utils.ConvertToDBString(xiCollection["ticketno"], Utils.DataType.Integer)));
            }

            if (xiCollection["status"] != null && xiCollection["status"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(t.status in ({0}))", xiCollection["status"]));
            }

            if (xiCollection["keyword"] != null && xiCollection["keyword"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(t.header like {0} or t.description like {0})", Utils.ConvertToDBString("%" + xiCollection["keyword"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["bug"] != null && xiCollection["bug"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(t.isbug = {0})", Utils.ConvertToDBString(xiCollection["bug"], Utils.DataType.Integer)));
            }

            if (xiCollection["priorityname"] != null && xiCollection["priorityname"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true; builder.Append(string.Format("(priorityname like {0})", Utils.ConvertToDBString("%" + xiCollection["priorityname"] + "%", Utils.DataType.String)));
            }
            if (xiCollection["voneeded"] != null && xiCollection["voneeded"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;

                switch (xiCollection["voneeded"])
                {
                    default:
                        builder.Append("(t.voneeded = 0 or t.voneeded is null)");
                        break;

                    case "1":
                        builder.Append("(t.voneeded = 1)");
                        break;
                }
            }

            return builder.ToString();
        }

        public static int GetTicketsHistoryCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(t.id) as maxcount from ticket_tickets t inner join [user] u1 on u1.id = t.createdby inner join [user] u2 on u2.id = t.updatedby inner join ticket_applications app on app.id = t.application inner join ticket_status s on s.id = t.status left join ticket_priority p on p.id = t.priority where t.active = 1 and t.status = 8";
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
                objdb.Write_log_file("GetTicketsHistoryCount", x.Message);
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

        public static DataSet GetTicketsHistory(int xiPage, string xiFilter, string xiSort)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"select t.id, t.ticketno, t.header, t.isbug, t.etd, t.created, t.[status],t.voneeded, t.[priority], t.createdby, t.created, t.completion_date,p.priorityname,
        t.lastmodified, s.[name] as statusname, app.name as appname, u1.lname as createdln, u1.fname as createdfn, u2.lname as updatedln, 
        u2.fname as updatedfn,
        (select count(*) from ticket_comments tc where tc.ticket_id = t.id) as commentscount,
        (select count(*) from ticket_documents tc where tc.ticket_id = t.id) as doccount,
        (select top 1 old_entry from ticket_log tl where tl.ticket_id = t.id and tl.message_id = t.status order by tl.datetime desc) as log_comments
        from ticket_tickets t inner join [user] u1 on u1.[id] = t.createdby 
        inner join [user] u2 on u2.[id] = t.updatedby 
        inner join ticket_applications app on app.id = t.[application]
        inner join ticket_status s on s.id = t.[status] left join ticket_priority p on p.id = t.priority where t.active = 1 and t.[status] = 8";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += "  order by " + GetSortOrder(xiSort) + " offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable newTable = Ticket.ProcessTicketTable(ds.Tables[0]);
                ds.Tables.RemoveAt(0);
                ds.Tables.Add(newTable);
            }

            return ds;
        }

        #endregion
       
        #region Ticket Log
        public bool AddLog(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into ticket_log(ticket_id, user_id, [datetime],message_id, old_entry, new_entry) values(";
            query += xiCollection["ticket_id"] + ", ";
            query += xiCollection["userid"] + ", ";
            query += "getutcdate(), ";
            query += xiCollection["messageid"] + ", ";
            query += Utils.ConvertToDBString(xiCollection["oldentry"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["newentry"], Utils.DataType.String);
            query += ")";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetTicketLogCount(string xiTicketId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(hl.message_id) as maxcount from ticket_log hl where hl.ticket_id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("Tickets-GetLogCount", x.Message);
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

        public static DataSet GetTicketLogs(int xiPage, string xiTicketId)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select hl.[user_id], hl.[datetime] as createddate, hl.message_id, hl.old_entry, hl.new_entry, (u.fname+' '+ u.lname)as processed_user, hl.ticket_id from ticket_log hl left join [user] u on u.[id] = hl.[user_id] where hl.ticket_id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer);
            query += " order by hl.[datetime] desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_remark", typeof(string));

                foreach (DataRow logRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate = Convert.ToDateTime(logRow["createddate"]);
                        logRow["processed_date"] = (tempDate != DateTime.MinValue) ? tempDate.ToString(BusinessBase.DateTimeFormat) : "";
                        logRow["processed_remark"] = Ticket.GetStatusLogText(logRow["message_id"], logRow["old_entry"]);
                        logRow["processed_user"] = BusinessBase.MaskText(logRow["processed_user"]);
                    }
                    catch { }
                }
            }

            return ds;
        }
        #endregion
    
        #region Ticket Dashboard
        public static DataSet GetTicketByStatus()
        {
            string query = @"select count(t.id) as ticketcount, ts.[name] from ticket_tickets t 
inner join ticket_status ts on ts.id = t.[status] where t.active = 1 group by ts.[name]";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataSet GetTicketByPriority()
        {
            string query = @"select count(t.id) as ticketcount, p.priorityname from ticket_tickets t
inner join ticket_priority p on t.[priority]=p.id where t.active = 1 and t.[status] != 8 group by p.priorityname";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataSet GetTicketByCategory(string xiStartDate, string xiEndDate)
        {
            DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue;
            try
            {
                startDate = Convert.ToDateTime(xiStartDate);
                endDate = Convert.ToDateTime(xiEndDate);
            }
            catch { }

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue) return null;

            xiStartDate = Utils.ConvertToDBString(xiStartDate + " 00:00:00", Utils.DataType.DateTime);
            xiEndDate = Utils.ConvertToDBString(xiEndDate + " 23:59:00", Utils.DataType.DateTime);

            string query = @"select count(t.id) as ticketcount, vsa.[name] as appname from ticket_tickets t 
inner join ticket_applications vsa on t.application=vsa.id where t.active = 1 and t.status!=8 and t.lastmodified between {0} and {1} group by vsa.[name]";
            query = string.Format(query, xiStartDate, xiEndDate);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataSet GetTicketOverview(string xiStartDate, string xiEndDate)
        {
            DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue;
            try
            {
                startDate = Convert.ToDateTime(xiStartDate);
                endDate = Convert.ToDateTime(xiEndDate);
            }
            catch { }

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue) return null;

            xiStartDate = Utils.ConvertToDBString(xiStartDate + " 00:00:00", Utils.DataType.DateTime);
            xiEndDate = Utils.ConvertToDBString(xiEndDate + " 23:59:00", Utils.DataType.DateTime);

            string query = @"select sa.[name] as appname,
(select sum(isnull(etd,0)) from ticket_tickets t1 where t1.[application]=sa.id and t1.active = 1 and t1.[status] = 6 and t1.lastmodified between {0} and {1}) as approvedtickets,
(select sum(isnull(etd,0)) from ticket_tickets t2 where t2.[application]=sa.id and t2.active = 1 and t2.[status] <> 6 and t2.[status] <> 8 and t2.lastmodified between {0} and {1}) as notapprovedtickets,
(select count(*) from ticket_tickets t3 where t3.[application]=sa.id and t3.active = 1 and t3.[status] <> 8 and t3.lastmodified between {0} and {1}) as totaltickets
from ticket_applications sa 
order by sa.[name]";

            query = string.Format(query, xiStartDate, xiEndDate);

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }
        #endregion
       
        #region VO Needed
        public bool UpdateVONeeded(NameValueCollection xiCollection, object xiTicketId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_tickets set ";
            query += "voneeded = " + Utils.ConvertToDBString(xiCollection["voneeded"], Utils.DataType.Integer) + ", ";
            query += "updatedby = " + Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer) + ", ";
            query += "lastmodified = getutcdate()";
            query += " where id = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer) + " and active=1;";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);

            if (value > 0)
            {
                string query1 = "delete from ticket_vo_approval_matrix_ticket where ticketid = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer);
                objdb.ExecuteNonQuery(objdb.con, query1); // if some has changed from 1 to 0, we need to delete the existing entry

                if (xiCollection["voneeded"] == "1")
                {
                    query1 = "insert into ticket_vo_approval_matrix_ticket(ticketid, matrix, matrix2, matrix3) select top 1 " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer) + ",matrix, matrix2, matrix3 from ticket_vo_approval_matrix where active=1";
                    objdb.ExecuteNonQuery(objdb.con, query1);
                }
            }

            objdb.Disconnectdb();

            return (value > 0);
        }
        #endregion

        #region VO Dist List

        public bool AddVODistListEmail(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into ticket_vo_maillist ([emailaddress]) values (";
            query += Utils.ConvertToDBString(xiCollection["emailaddress"], Utils.DataType.String);
            query += ")";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateVODistListEmail(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update ticket_vo_maillist set ";
            query += "[emailaddress] = " + Utils.ConvertToDBString(xiCollection["emailaddress"], Utils.DataType.String);
            query += " where id = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }



        public static NameValueCollection GetVOEmail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,emailaddress from ticket_vo_maillist where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("emailaddress", Convert.ToString(objdb.GetValue(reader1, "emailaddress")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetVOEmail", x.Message);
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

        public static bool DeleteVOEmail(int xiId)
        {
            string query = string.Format("delete from ticket_vo_maillist where id = {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataSet GetVOEmails(string xiFilter)
        {
            string query = "select e.* from ticket_vo_maillist e where e.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by e.id";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }
        public static DataTable GetAllVODistListEmails()
        {
            string query = "select e.* from ticket_vo_maillist e where e.active = 1 order by e.id";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        #endregion

        #region VO Approval Matrix

        public static DataTable GetUsers()
        {
            //string query = "select distinct s.user_id,(s.user_pre_name + ' ' + s.user_family_name) as user_full_name, s.user_email as user_email, s.user_phone as user_phone, a.id as app_id from [system_user] s inner join system_user_department ud on s.user_id = ud.user_id  inner join system_applications a on ud.app_id = a.id where s.delete_user_id = 0 and s.user_status = 1 and a.foldername = " + Utils.ConvertToDBString(xiApplicationName, Utils.DataType.String) + " order by user_full_name";
            string query = "select distinct u.id as user_id,(u.fname + ' ' + u.lname) as user_full_name, u.fname as fname, u.lname as lname, u.email as email, u.phone as phone,u.type,u.submitdate,u.isowner from[user] u where u.isowner = 1 and u.active = 1 order by user_full_name";
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public bool SaveVOApprovalMatrix(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = @"if not exists(select 1 from ticket_vo_approval_matrix where active=1) 
insert into ticket_vo_approval_matrix(matrix,matrix2,matrix3,active) values(@matrix,@matrix2,@matrix3,1)
else
update ticket_vo_approval_matrix set matrix=@matrix,matrix2=@matrix2,matrix3=@matrix3 where active=1";

            Parameter param1 = new Parameter("matrix", xiCollection["matrix"]);
            Parameter param2 = new Parameter("matrix2", xiCollection["matrix2"]);
            Parameter param3 = new Parameter("matrix3", xiCollection["matrix3"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetVOApprovalMatrix()
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select 
nm.*,
(select stuff((select ', ' + ad.fname + ' ' + ad.lname as user_fullname from [user] ad where ad.[id] in(select value from ticket_vo_approval_matrix hu3 CROSS APPLY STRING_SPLIT_2(SUBSTRING (hu3.matrix ,0 ,LEN(hu3.matrix )+100), ',')) for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as matrix_name,
(select stuff((select ', ' + ad1.fname + ' ' + ad1.lname as user_fullname from [user] ad1 where ad1.[id] in(select value from ticket_vo_approval_matrix hu3 CROSS APPLY STRING_SPLIT_2(SUBSTRING (hu3.matrix2 ,0 ,LEN(hu3.matrix2 )+100), ',')) for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as matrix2_name,
(select stuff((select ', ' + ad2.fname + ' ' + ad2.lname as user_fullname from [user] ad2 where ad2.[id] in(select value from ticket_vo_approval_matrix hu3 CROSS APPLY STRING_SPLIT_2(SUBSTRING (hu3.matrix3 ,0 ,LEN(hu3.matrix3 )+100), ',')) for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as matrix3_name
from ticket_vo_approval_matrix nm where nm.active=1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("matrix", Convert.ToString(objdb.GetValue(reader1, "matrix")));
                    collection.Add("matrix2", Convert.ToString(objdb.GetValue(reader1, "matrix2")));
                    collection.Add("matrix3", Convert.ToString(objdb.GetValue(reader1, "matrix3")));
                    collection.Add("matrix_name", Convert.ToString(objdb.GetValue(reader1, "matrix_name")));
                    collection.Add("matrix2_name", Convert.ToString(objdb.GetValue(reader1, "matrix2_name")));
                    collection.Add("matrix3_name", Convert.ToString(objdb.GetValue(reader1, "matrix3_name")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetVOApprovalMatrix", x.Message);
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

        #region VO Approval

        public bool SaveVOApproval(NameValueCollection xiCollection, object xiTicketId)
        {
            if (xiCollection == null) return false;

            string query = "insert into ticket_vo_approval_matrix_ticket_approve(ticketid,user_id,approvelevel,last_action) values(@ticketid,@userid,@level,getutcdate());";

            Parameter param1 = new Parameter("ticketid", xiTicketId, DbType.Int32);
            Parameter param2 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param3 = new Parameter("level", xiCollection["level"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            if (value > 0)
            {
                NameValueCollection logCollection = new NameValueCollection();
                logCollection["ticket_id"] = xiTicketId.ToString();
                logCollection["userid"] = xiCollection["userid"];
                logCollection["messageid"] = ((int)Status.INDIVIDUALUSERVOAPPROVED).ToString();
                logCollection["oldentry"] = xiCollection["level"];
                logCollection["newentry"] = "-";
                this.AddLog(logCollection);
            }

            return (value > 0);
        }

        public bool WithdrawVOApproval(object xiTicketId, object xiUserId)
        {
            string query = "delete from ticket_vo_approval_matrix_ticket_approve where ticketid=@ticketid and userid=@userid";

            Parameter param1 = new Parameter("ticketid", xiTicketId, DbType.Int32);
            Parameter param2 = new Parameter("userid", xiUserId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetTicketVOApprovedComments(object xiTicketId)
        {
            string query = @"select ta.*, (su.fname + ' ' + su.lname) as user_name
from ticket_vo_approval_matrix_ticket_approve ta inner join [user] su on su.[id]=ta.[user_id]
where ta.ticketid= " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static NameValueCollection GetTicketVOApprovalMatrix(object xiTicketId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            string query = @"select matrix,matrix2,matrix3, 
(select stuff((select ', ' + ad.fname + ' ' + ad.lname as user_fullname from [user] ad where ad.[id] in(select value from ticket_vo_approval_matrix_ticket hu3 CROSS APPLY STRING_SPLIT_2(SUBSTRING (hu3.matrix ,0 ,LEN(hu3.matrix )+100), ',') where hu3.ticketid=nm.ticketid) for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as matrix_name,
(select stuff((select ', ' + ad1.fname + ' ' + ad1.lname as user_fullname from [user] ad1 where ad1.[id] in(select value from ticket_vo_approval_matrix_ticket hu3 CROSS APPLY STRING_SPLIT_2(SUBSTRING (hu3.matrix2 ,0 ,LEN(hu3.matrix2 )+100), ',') where hu3.ticketid=nm.ticketid) for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as matrix2_name,
(select stuff((select ', ' + ad2.fname + ' ' + ad2.lname as user_fullname from [user] ad2 where ad2.[id] in(select value from ticket_vo_approval_matrix_ticket hu3 CROSS APPLY STRING_SPLIT_2(SUBSTRING (hu3.matrix3 ,0 ,LEN(hu3.matrix3 )+100), ',') where hu3.ticketid=nm.ticketid) for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as matrix3_name
from ticket_vo_approval_matrix_ticket nm 
where nm.ticketid = " + Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("matrix", Convert.ToString(objdb.GetValue(reader1, "matrix")));
                    collection.Add("matrix2", Convert.ToString(objdb.GetValue(reader1, "matrix2")));
                    collection.Add("matrix3", Convert.ToString(objdb.GetValue(reader1, "matrix3")));
                    collection.Add("matrix_name", Convert.ToString(objdb.GetValue(reader1, "matrix_name")));
                    collection.Add("matrix2_name", Convert.ToString(objdb.GetValue(reader1, "matrix2_name")));
                    collection.Add("matrix3_name", Convert.ToString(objdb.GetValue(reader1, "matrix3_name")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetTicketVOApprovalMatrix", x.Message);
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

        public static DataTable GetTicketVOApprovedComments1(object xiTicketId)
        {
            string query = @"select su.id, pa.last_action, su.fname + ' ' + su.lname + ' ' + (case 
when pm.matrix+',' like '%'+cast(su.[id] as nvarchar)+',%' then '(L1)'
when pm.matrix2+',' like '%'+cast(su.[id] as nvarchar)+',%' then '(L2)'
when pm.matrix3+',' like '%'+cast(su.[id] as nvarchar)+',%' then '(L3)'
end) as user_full_name
from [user] su 
inner join ticket_vo_approval_matrix_ticket pm on pm.[ticketid] ={0}
and (
','+pm.matrix+',' like '%,'+cast(su.[id] as nvarchar)+',%' 
or ','+pm.matrix2+',' like '%,'+cast(su.[id] as nvarchar)+',%' 
or ','+pm.matrix3+',' like '%,'+cast(su.[id] as nvarchar)+',%') 
left join ticket_vo_approval_matrix_ticket_approve pa on pa.[ticketid]=pm.[ticketid] and pa.[user_id] = su.[id] 
where pm.ticketid={0} order by (case 
when ','+pm.matrix+',' like '%,'+cast(su.[id] as nvarchar)+',%' then 1
when ','+pm.matrix2+',' like '%,'+cast(su.[id] as nvarchar)+',%' then 2
when ','+pm.matrix3+',' like '%,'+cast(su.[id] as nvarchar)+',%' then 3
end),su.fname + ' ' + su.lname";
            query = string.Format(query, Utils.ConvertToDBString(xiTicketId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        #endregion
    }
}
