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
    public class EventBA : BusinessBase
    {
        #region Event

        public int AddEvent(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [event]([title],[venue],[description],[startdate],[enddate],[terms_condition],[animalcategory],[breedtype],[banner_image],[active],[submitdate],[createdby],visible) values(@title,@venue,@description,@startdate,@enddate,@terms_condition,@animalcategory,@breedtype,@banner_image,1,getutcdate(),@userid,@visible)";

            Parameter param1 = new Parameter("title", xiCollection["title"]);
            Parameter param2 = new Parameter("venue", xiCollection["venue"]);
            Parameter param3 = new Parameter("description", xiCollection["description"]);
            Parameter param4 = new Parameter("startdate", xiCollection["startdatetime"], DbType.DateTime);
            Parameter param5 = new Parameter("enddate", xiCollection["enddatetime"], DbType.DateTime);
            Parameter param6 = new Parameter("terms_condition", xiCollection["terms_condition"]);
            Parameter param7 = new Parameter("animalcategory", xiCollection["animalcategory"], DbType.Int32);
            Parameter param8 = new Parameter("breedtype", xiCollection["breedtype"], DbType.String);
            Parameter param9 = new Parameter("banner_image", xiCollection["bannerimage"]);
            Parameter param10 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param11 = new Parameter("visible", xiCollection["visible"]);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11 });
            objdb.Disconnectdb();

            return value;
        }

        public bool UpdateEvent(NameValueCollection xiCollection, object xiEventId)
        {
            if (xiCollection == null) return false;

            string query = "update [event] set title=@title,venue=@venue,description=@description,startdate=@startdate,enddate=@enddate,terms_condition=@terms_condition,animalcategory=@animalcategory,breedtype=@breedtype,banner_image=@banner_image,submitdate=getutcdate(),visible=@visible where id=@id";
            Parameter param1 = new Parameter("title", xiCollection["title"]);
            Parameter param2 = new Parameter("venue", xiCollection["venue"]);
            Parameter param3 = new Parameter("description", xiCollection["description"]);
            Parameter param4 = new Parameter("startdate", xiCollection["startdatetime"], DbType.DateTime);
            Parameter param5 = new Parameter("enddate", xiCollection["enddatetime"], DbType.DateTime);
            Parameter param6 = new Parameter("terms_condition", xiCollection["terms_condition"]);
            Parameter param7 = new Parameter("animalcategory", xiCollection["animalcategory"], DbType.Int32);
            Parameter param8 = new Parameter("breedtype", xiCollection["breedtype"], DbType.String);
            Parameter param9 = new Parameter("banner_image", xiCollection["bannerimage"]);
            Parameter param10 = new Parameter("id", xiEventId, DbType.Int32);
            Parameter param11 = new Parameter("visible", xiCollection["visible"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetEventDetail(object xiEventId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select e.*,(select count(*) from event_registrations where eventid = e.id) as registrationcount, u.fname+''+u.lname as 'eventowner', u.email as 'owneremail' from [event] e inner join [user] u on e.createdby = u.id where e.active=1 and u.active = 1 and e.id=" + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("title", Convert.ToString(objdb.GetValue(reader1, "title")));
                    collection.Add("venue", Convert.ToString(objdb.GetValue(reader1, "venue")));
                    collection.Add("description", Convert.ToString(objdb.GetValue(reader1, "description")));
                    collection.Add("startdate", Convert.ToString(objdb.GetValue(reader1, "startdate")));
                    collection.Add("enddate", Convert.ToString(objdb.GetValue(reader1, "enddate")));
                    collection.Add("terms_condition", Convert.ToString(objdb.GetValue(reader1, "terms_condition")));
                    collection.Add("animalcategory", Convert.ToString(objdb.GetValue(reader1, "animalcategory")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("banner_image", Convert.ToString(objdb.GetValue(reader1, "banner_image")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("registrationcount", Convert.ToString(objdb.GetValue(reader1, "registrationcount")));
                    collection.Add("visible", Convert.ToString(objdb.GetValue(reader1, "visible")));
                    collection.Add("eventownername", Convert.ToString(objdb.GetValue(reader1, "eventowner")));
                    collection.Add("eventowneremail", Convert.ToString(objdb.GetValue(reader1, "owneremail")));

                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetEventDetail", x.Message);
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

        public static string DeleteEventReason(object xiId, string xiDeleteReason)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string query = @"select count(er.eventid) as maxcount from event_registrations er 
                inner join [event] e on e.id=er.eventid where er.eventid = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and e.active=1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("DeleteEvent", x.Message);
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
                query = string.Format("update [event] set active = 0 ,[delete_reason] = {1} where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer), Utils.ConvertToDBString(xiDeleteReason, Utils.DataType.String));

                objdb.Connectdb();
                int value = objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();

                returnValue = (value > 0) ? "Deleted Successfully." : "false";
            }
            else
            {
                returnValue = "You can not delete this event because it is already in used.";
            }
            objdb.Disconnectdb();

            return returnValue;
        }

        public static string DeleteEvent(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string query = @"select count(er.eventid) as maxcount from event_registrations er 
                inner join [event] e on e.id=er.eventid where er.eventid = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and e.active=1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("DeleteEvent", x.Message);
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
                query = string.Format("update [event] set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

                objdb.Connectdb();
                int value = objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();

                returnValue = (value > 0) ? "Deleted Successfully." : "false";
            }
            else
            {
                returnValue = "You can not delete this event because its already used in used.";
            }
            objdb.Disconnectdb();

            return returnValue;
        }


        public static int GetEventCount(string xiFilter, object xiUserId)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(e.id) as maxcount from [event] e where e.active = 1";
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
                objdb.Write_log_file("GetEventCount", x.Message);
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

        public static DataSet GetEventDetails(int xiPage, string xiFilter, object xiUserId)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"select *,(select count(*) from event_registrations where eventid = e.id) as registrationcount,
(select count(*) from event_registrations where eventid = e.id and userid= " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + @") as myregistrationcount
from [event] e where e.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by startdate offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_datetime", typeof(string));
                ds.Tables[0].Columns.Add("procesed_date", typeof(string));
                ds.Tables[0].Columns.Add("procesed_time", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["startdate"]);
                    DateTime tempDate2 = Convert.ToDateTime(row["enddate"]);

                    string eventdatetime = string.Empty;
                    string eventdate = string.Empty;
                    string eventtime = string.Empty;
                    if (tempDate1 != DateTime.MinValue)
                    {
                        eventdatetime = tempDate1.ToString(BusinessBase.DateTimeFormat);
                        eventdate = tempDate1.ToString(BusinessBase.DateFormat);
                        eventtime = tempDate1.ToString("HH:mm");

                        if (tempDate2 != DateTime.MinValue)
                        {
                            if (DateTime.Compare(tempDate1.Date, tempDate2.Date) == 0)
                            {
                                eventdatetime += "  - " + tempDate2.ToString("HH:mm");
                                eventtime += "  - " + tempDate2.ToString("HH:mm");
                            }
                            else
                            {
                                eventdatetime += "  - " + tempDate2.ToString(BusinessBase.DateTimeFormat);
                                eventdate += "  - " + tempDate2.ToString(BusinessBase.DateFormat);
                                eventtime += "  - " + tempDate2.ToString("HH:mm");
                            }
                        }
                    }
                    row["procesed_datetime"] = eventdatetime;
                    row["procesed_date"] = eventdate;
                    row["procesed_time"] = eventtime;
                }
            }

            return ds;
        }

        public static string EventSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["owner"] != null && xiCollection["owner"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(e.owner={0})", Utils.ConvertToDBString(xiCollection["owner"], Utils.DataType.Integer)));
            }

            if (xiCollection["animalcategory"] != null && xiCollection["animalcategory"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(e.animalcategory={0})", Utils.ConvertToDBString(xiCollection["animalcategory"], Utils.DataType.Integer)));
            }

            if (xiCollection["breedtype"] != null && xiCollection["breedtype"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(e.breedtype is null or ','+e.breedtype+',' like {0})", Utils.ConvertToDBString("%," + xiCollection["breedtype"] + ",%", Utils.DataType.String)));
            }

            if (xiCollection["title"] != null && xiCollection["title"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(e.title like {0})", Utils.ConvertToDBString("%" + xiCollection["title"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["userid"] != null && xiCollection["userid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;

                builder.Append(string.Format(@"(e.breedtype is null or e.id in (select e1.id FROM event e1 CROSS APPLY STRING_SPLIT(SUBSTRING (e1.breedtype ,0 ,LEN(e1.breedtype)+1000), ',')
where e1.active = 1 and value in (select distinct a.breedtype from animal a inner join user_animal ua on a.id = ua.animalid and ua.userid = {0})))", Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer)));

                builder.Append(" and ");

                builder.Append(string.Format(@"(e.createdby={0} or e.id in (select id from event e1 where e1.active=1 and e1.visible is null) or ((select type from [user] where id={0}) =
(case
	when e.id in (select e1.id from event e1 where e1.active=1 and ','+e1.visible+',' like '%,1,%') then 1
	when e.id in (select e1.id from event e1 where e1.active=1 and ','+e1.visible+',' like '%,2,%') then 2
	else 0
end))
)", xiCollection["userid"]));

            }

            /*if (xiCollection["associateid"] != null && xiCollection["associateid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;

                builder.Append(string.Format(@"( {0} in (select isnull(visible_userid,{0}) from view_event_users eu where eu.id = e.id))", Utils.ConvertToDBString(xiCollection["associateid"], Utils.DataType.Integer)));
            }*/

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
                    string startdate = Utils.ConvertToDBString(startDate.ToString(BusinessBase.DateFormat) + " 00:00", Utils.DataType.DateTime);
                    string enddate = Utils.ConvertToDBString(endDate.ToString(BusinessBase.DateFormat) + " 23:59", Utils.DataType.DateTime);
                    builder.Append(string.Format("(e.enddate between {0} and {1})", startdate, enddate));
                }
                catch { }
            }
            else
            {
                if (xiCollection["enddate"] != null && xiCollection["enddate"].Length > 0)
                {
                    if (iswhere) builder.Append(" and ");
                    iswhere = true;
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                        DateTime startDate = Convert.ToDateTime(xiCollection["enddate"], CultureInfo.CurrentCulture);
                        string startdate = Utils.ConvertToDBString(startDate.ToString(BusinessBase.DateFormat), Utils.DataType.Date);
                        builder.Append(string.Format("e.enddate >= {0}", startdate));
                    }
                    catch { }
                }
            }

            return builder.ToString();
        }

        public bool AddFiles(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [event_files]([eventid],[file]) values(@eventid,@file)";
            Parameter param1 = new Parameter("eventid", xiCollection["eventid"], DbType.Int32);
            Parameter param2 = new Parameter("file", xiCollection["file"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetFilesDetails(object xiEventId)
        {
            string query = "select * from event_files where eventid=" + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("file_type", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string extension = string.Empty;
                    string fileVal = Convert.ToString(row["file"]);
                    if (!string.IsNullOrEmpty(fileVal))
                    {
                        string filePath = @"docs/" + fileVal;
                        extension = System.IO.Path.GetExtension(filePath);

                        switch (extension)
                        {
                            case ".pdf":
                            case ".doc":
                            case ".docx":
                            case ".xls":
                            case ".xlsx":
                                extension = "1";
                                break;

                            default:
                                extension = "2";
                                break;

                        }
                    }
                    row["file_type"] = extension;
                }
            }
            return ds.Tables[0];
        }

        public static DataTable GetEventPublishBrochure(object xiEventId)
        {
            string query = " Select * from event_brochure where active = 1 and [status] = 2 and eventid= " + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dt = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dt;
        }

        public bool AssignOtherFields(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = @"if not exists(select 1 from event_customfields where eventid=@eventid and fieldid=@fieldid) insert into event_customfields(eventid,fieldid) values(@eventid,@fieldid)";
            Parameter param1 = new Parameter("eventid", xiCollection["eventid"], DbType.Int32);
            Parameter param2 = new Parameter("fieldid", xiCollection["fieldid"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UnAssignOtherFields(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = @"delete from event_customfields where eventid=@eventid and fieldid=@fieldid";
            Parameter param1 = new Parameter("eventid", xiCollection["eventid"], DbType.Int32);
            Parameter param2 = new Parameter("fieldid", xiCollection["fieldid"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetAssignedOtherFields(object xiEventId)
        {
            string query = @"select cf.eventid, c.*,cv.fieldvalue,(select stuff(( 
select ', ' + co.optiontext from animal_customfieldsoption co where co.id in
(select value from animal_customfields_values cv1
CROSS APPLY STRING_SPLIT(SUBSTRING (cv1.fieldvalue ,0 ,LEN(cv1.fieldvalue)+100), ',') where cv1.id = cv.id and cv.fieldid in (select id from animal_customfields where [type] in (3,4,5)))
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as optiontext
from event_customfields cf
inner join animal_customfields c on c.id = cf.fieldid
left join animal_customfields_values cv on cv.fieldid = c.id and cv.animalid = " + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer) + @"
where c.active = 1 and c.breedtype='0' and cf.eventid = " + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer) + " order by c.sortorder,c.title";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string EventListSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["title"] != null && xiCollection["title"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(e.title like {0})", Utils.ConvertToDBString("%" + xiCollection["title"] + "%", Utils.DataType.String)));
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
                    string startdate = Utils.ConvertToDBString(startDate.ToString(BusinessBase.DateFormat) + " 00:00", Utils.DataType.DateTime);
                    string enddate = Utils.ConvertToDBString(endDate.ToString(BusinessBase.DateFormat) + " 23:59", Utils.DataType.DateTime);
                    builder.Append(string.Format("(e.enddate between {0} and {1})", startdate, enddate));
                }
                catch { }
            }
            else
            {
                if (xiCollection["enddate"] != null && xiCollection["enddate"].Length > 0)
                {
                    if (iswhere) builder.Append(" and ");
                    iswhere = true;
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                        DateTime startDate = Convert.ToDateTime(xiCollection["enddate"], CultureInfo.CurrentCulture);
                        string startdate = Utils.ConvertToDBString(startDate.ToString(BusinessBase.DateFormat), Utils.DataType.Date);
                        builder.Append(string.Format("e.enddate >= {0}", startdate));
                    }
                    catch { }
                }
            }

            if (string.IsNullOrEmpty(xiCollection["superadmin"]) && xiCollection["superadmin"] != "1")
            {
                if (xiCollection["userid"] != null && xiCollection["userid"].Length > 0)
                {
                    if (iswhere) builder.Append(" and ");
                    iswhere = true;
                    builder.Append(string.Format("(e.createdby={0})", Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer)));
                }
            }

            return builder.ToString();
        }

        #endregion

        #region Event_Registrations

        public int RegisterUser(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "if not exists(select 1 from event_registrations where eventid=@eventid and userid=@userid) insert into event_registrations(eventid,userid,submitdate,animallist) values(@eventid,@userid,getutcdate(),@animallist)";

            Parameter param1 = new Parameter("eventid", xiCollection["eventid"], DbType.Int32);
            Parameter param2 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param3 = new Parameter("animallist", xiCollection["animallist"]);

            string query2 = "select scope_identity()";


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool IsUserRegistered(object xiEventId, object xiUserId)
        {
            string query = "select 1 from event_registrations where eventid =" + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer) + " and userid= " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object obj = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return (BusinessBase.ConvertToInteger(obj) > 0);
        }

        public static NameValueCollection GetRegisterUserDetail(object xiRegistrationId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select e.* from event_registrations e where e.id=" + Utils.ConvertToDBString(xiRegistrationId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("eventid", Convert.ToString(objdb.GetValue(reader1, "eventid")));
                    collection.Add("userid", Convert.ToString(objdb.GetValue(reader1, "userid")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetEventDetail", x.Message);
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


        public static int GetRegisteredUserCount(string xiEventId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(er.id) as maxcount from event_registrations er inner join [user] u on er.userid = u.id where er.eventid = " + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetEvent_RegistrationsCount", x.Message);
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

        public static DataSet GetRegisteredUsers(int xiPage, string xiEventId)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select er.id as eventregistrationid, u.fname as fname, u.lname as lname, u.email as email, u.phone as phone, er.eventid, er.userid, er.submitdate  
from event_registrations er 
inner join [user] u on er.userid = u.id where er.eventid = " + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer);
            query += " order by u.fname, u.lname offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_date", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_date"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                    }
                }
            }

            return ds;
        }

        public static DataTable GetRegisteredOtherFields(object xiRegistrationId)
        {
            NameValueCollection regCollection = EventBA.GetRegisterUserDetail(xiRegistrationId);
            if (regCollection == null) return null;


            string query = @"select cf.eventid, c.*,cv.fieldvalue,(select stuff(( 
select ', ' + co.optiontext from animal_customfieldsoption co where co.id in
(select value from animal_customfields_values cv1
CROSS APPLY STRING_SPLIT(SUBSTRING (cv1.fieldvalue ,0 ,LEN(cv1.fieldvalue)+100), ',') where cv1.id = cv.id and cv.fieldid in (select id from animal_customfields where [type] in (3,4,5)))
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as optiontext
from event_customfields cf
inner join animal_customfields c on c.id = cf.fieldid
left join animal_customfields_values cv on cv.fieldid = c.id and cv.animalid = " + Utils.ConvertToDBString(xiRegistrationId, Utils.DataType.Integer) + @"
where c.active = 1 and c.breedtype='0' and cf.eventid = " + Utils.ConvertToDBString(regCollection["eventid"], Utils.DataType.Integer) + " order by c.sortorder,c.title";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        //#TODO Testing
        public bool DeRegisterUser(object xiEventId, object UserId)
        {
            string query = string.Format("delete event_registrations where eventid = {0} and userid = {1}", Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer), Utils.ConvertToDBString(UserId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }


        #endregion

        public static NameValueCollection GetAssociatsUser(object xiAssociatsUserId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select asso_id from associate_user where userid = " + Utils.ConvertToDBString(xiAssociatsUserId, Utils.DataType.Integer) + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();

                    collection.Add("asso_id", Convert.ToString(objdb.GetValue(reader1, "asso_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAssociatsUser", x.Message);
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

        public static DataTable GetAllAssociats(string xiEventId)
        {
            string query = "select userid from associate_user where asso_id=" + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dataTable;
        }

        public static bool DeleteEventPhoto(string xiFileName)
        {
            string query = "delete from [event_files] where [file]=@file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();

            return (value > 0);

        }

        #region Event Brochure

        public enum ContentType
        {
            TEXTEDITOR = 1,
            ANIMAL = 2,
            OWNER = 3,
            SPONSOR = 4,
        }

        public enum SponsorType
        {
            GOLD = 1,
            SILVER = 2,
            BRONZE = 3,
        }

        public int AddBrochure(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [event_brochure](eventid,[name],[description],[submitdate],[active],headertext,footertext,status) values(@eventid,@name,@description,getutcdate(),1,@headertext,@footertext,@status)";
            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("description", xiCollection["description"]);
            Parameter param3 = new Parameter("eventid", xiCollection["eventid"]);
            Parameter param4 = new Parameter("headertext", xiCollection["headertext"]);
            Parameter param5 = new Parameter("footertext", xiCollection["footertext"]);
            Parameter param6 = new Parameter("status", xiCollection["status"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query2 = "select scope_identity()";
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6 });
            objdb.Disconnectdb();

            if (value > 0)
                return value;
            else
                return int.MinValue;
        }

        public bool UpdateBrochure(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update event_brochure set name=@name,description=@description,headertext=@headertext,footertext=@footertext,status=@status where active=1 and id=@id";
            Parameter param2 = new Parameter("name", xiCollection["name"]);
            Parameter param3 = new Parameter("description", xiCollection["description"]);
            Parameter param4 = new Parameter("headertext", xiCollection["headertext"]);
            Parameter param5 = new Parameter("footertext", xiCollection["footertext"]);
            Parameter param6 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param7 = new Parameter("id", xiId);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2, param3, param4, param5, param6, param7 });
            objdb.Disconnectdb();
            return (value > 0);
        }

        public bool UpdateBrochurePic(string xiFileName, object xiId)
        {
            string query = "update event_brochure set brochure_file=@brochure_file where id = @id and active=1";

            Parameter param1 = new Parameter("brochure_file", xiFileName);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }
        
        public static bool PublishBrochure(object xiId, object xiStatus)
        {
            int status = BusinessBase.ConvertToInteger(xiStatus);
            string query = "";

            if (status == 1)
                query = "update event_brochure set status = 1 where id = @id and active=1 and status = 2";
            else if (status == 2)
                query = "update event_brochure set status = 2 where id = @id and active=1 and status = 1";
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetBrochureDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from [event_brochure] where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("description", Convert.ToString(objdb.GetValue(reader1, "description")));
                    collection.Add("eventid", Convert.ToString(objdb.GetValue(reader1, "eventid")));
                    collection.Add("brochure_file", Convert.ToString(objdb.GetValue(reader1, "brochure_file")));
                    collection.Add("headertext", Convert.ToString(objdb.GetValue(reader1, "headertext")));
                    collection.Add("footertext", Convert.ToString(objdb.GetValue(reader1, "footertext")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBrochureDetail", x.Message);
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

        public static int GetEventBrochureCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from [event_brochure] c inner join [event] e on c.eventid = e.id where c.active = 1 and e.active = 1";
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
                objdb.Write_log_file("GetEventBrochureCount", x.Message);
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

        public static DataSet GetEventBrochureDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;
            string query = "select c.* from [event_brochure] c inner join [event] e on c.eventid = e.id where c.active = 1 and e.active = 1 ";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.[name] offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static bool DeleteEventBrochure(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update [event_brochure] set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string BrochureSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["eventid"] != null && xiCollection["eventid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.eventid={0})", Utils.ConvertToDBString(xiCollection["eventid"], Utils.DataType.Integer)));
            }

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.[name] like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            return builder.ToString();
        }

        public static DataTable GetsponsorsByType(string xiTypeId, object xiEventId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select id, [name], [description], eventid from [event_sponsors] where active = 1 and [type] = " + Utils.ConvertToDBString(xiTypeId, Utils.DataType.Integer) + " and eventid = " + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer) + " ";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public int AddBrochurePage(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = @"INSERT INTO [event_brochure_pages](brochureid,pagename,sequence_no,content_type,content_subtype,content_data,active,[submitdate],[createdby]) 
values(@brochureid,@pagename,(select ISNULL(Max(sequence_no),0) + 1 as 'sequ_no' from [event_brochure_pages] where brochureid = @brochureid ),@content_type,@content_subtype,
@content_data,1,getutcdate(),@createdby)";

            Parameter param1 = new Parameter("brochureid", xiCollection["brochureid"]);
            Parameter param2 = new Parameter("pagename", xiCollection["pagename"]);
            Parameter param3 = new Parameter("sequence_no", xiCollection["sequence_no"], DbType.Int32);
            Parameter param4 = new Parameter("content_type", xiCollection["content_type"], DbType.Int16);
            Parameter param5 = string.IsNullOrEmpty(xiCollection["content_subtype"]) ? new Parameter("content_subtype", DBNull.Value) : new Parameter("content_subtype", xiCollection["content_subtype"], DbType.Int16);
            Parameter param6 = string.IsNullOrEmpty(xiCollection["content_data"]) ? new Parameter("content_data", DBNull.Value) : new Parameter("content_data", xiCollection["content_data"]);
            Parameter param7 = new Parameter("createdby", xiCollection["createdby"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query2 = "select scope_identity()";
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7 });
            objdb.Disconnectdb();

            if (value > 0)
                return value;
            else
                return int.MinValue;
        }

        public bool UpdateBrochurePage(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = @"update event_brochure_pages set pagename=@pagename,content_type=@content_type,content_subtype=@content_subtype,content_data=@content_data
                where active=1 and id=@id";

            Parameter param1 = new Parameter("pagename", xiCollection["pagename"]);
            Parameter param2 = new Parameter("content_type", xiCollection["content_type"], DbType.Int16);
            Parameter param3 = string.IsNullOrEmpty(xiCollection["content_subtype"]) ? new Parameter("content_subtype", DBNull.Value) : new Parameter("content_subtype", xiCollection["content_subtype"], DbType.Int16);
            Parameter param4 = string.IsNullOrEmpty(xiCollection["content_data"]) ? new Parameter("content_data", DBNull.Value) : new Parameter("content_data", xiCollection["content_data"]);
            Parameter param5 = new Parameter("id", xiId);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5 });
            objdb.Disconnectdb();
            return (value > 0);
        }

        public static NameValueCollection GetBrochurePageDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from [event_brochure_pages] bp inner join [event_brochure] b on bp.brochureid = b.id where bp.active=1 and b.active = 1 and bp.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("brochureid", Convert.ToString(objdb.GetValue(reader1, "brochureid")));
                    collection.Add("pagename", Convert.ToString(objdb.GetValue(reader1, "pagename")));
                    collection.Add("sequence_no", Convert.ToString(objdb.GetValue(reader1, "sequence_no")));
                    collection.Add("content_type", Convert.ToString(objdb.GetValue(reader1, "content_type")));
                    collection.Add("content_subtype", Convert.ToString(objdb.GetValue(reader1, "content_subtype")));
                    collection.Add("content_data", Convert.ToString(objdb.GetValue(reader1, "content_data")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBrochureDetail", x.Message);
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

        public static int GetBrochurePageCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(bp.id) as maxcount from [event_brochure_pages] bp inner join [event_brochure] b on bp.brochureid = b.id where bp.active=1 and b.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + "bp.brochureid =" + Utils.ConvertToDBString(xiFilter, Utils.DataType.Integer);
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
                objdb.Write_log_file("GetEventBrochureCount", x.Message);
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

        public static DataSet GetBrochurePageListDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;
            string query = "select bp.*,b.[name] as 'brochurename' from [event_brochure_pages] bp inner join [event_brochure] b on bp.brochureid = b.id where bp.active=1 and b.active = 1 ";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + "bp.brochureid =" + Utils.ConvertToDBString(xiFilter, Utils.DataType.Integer);

            query += " order by bp.[pagename] offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_date", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_date"] = tempDate1.ToString(BusinessBase.DateFormat);
                    }
                }
            }

            objdb.Disconnectdb();
            return ds;
        }

        public static DataTable GetAllBrochurePages(object xiBrochureid)
        {
            string query = "select bp.id, bp.pagename,bp.brochureid,bp.sequence_no,bp.content_type,bp.content_subtype,bp.createdby,bp.submitdate,b.[name] 'brname' ,b.[description] 'brdesc', b.brochure_file 'brfile' from [event_brochure_pages] bp inner join [event_brochure] b on bp.brochureid = b.id where bp.active=1 and b.active = 1 and bp.brochureid = " + Utils.ConvertToDBString(xiBrochureid, Utils.DataType.Integer) + " order by sequence_no";
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dt = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dt;
        }

        public static bool DeleteBrochurePage(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update [event_brochure_pages] set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        #endregion

        //new 18 dec 2023 nilesh
        #region Event Sponsor
        public int AddSponsor(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [event_sponsors](eventid,[name],[description],[type],[sponsor_file],[submitdate],[active]) values(@eventid,@name,@description,@type,@sponsor_file,getutcdate(),1)";
            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("description", xiCollection["description"]);
            Parameter param3 = new Parameter("eventid", xiCollection["eventid"], DbType.Int32);
            Parameter param4 = new Parameter("type", xiCollection["type"], DbType.Int32);
            Parameter param5 = new Parameter("sponsor_file", xiCollection["sponsor_file"]);
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query2 = "select scope_identity()";
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5 });
            objdb.Disconnectdb();

            if (value > 0)
                return value;
            else
                return int.MinValue;
        }

        public bool UpdateSponsor(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update event_sponsors set name=@name,description=@description,type=@type,sponsor_file=@sponsor_file,eventid=@eventid where active=1 and id=@id";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("description", xiCollection["description"]);
            Parameter param3 = new Parameter("type", xiCollection["type"], DbType.Int32);
            Parameter param4 = new Parameter("sponsor_file", xiCollection["sponsor_file"]);
            Parameter param5 = new Parameter("eventid", xiCollection["eventid"], DbType.Int32);
            Parameter param6 = new Parameter("id", xiId, DbType.Int32);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6 });
            objdb.Disconnectdb();
            return (value > 0);
        }

        public static NameValueCollection GetSponsorDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from [event_sponsors] where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("description", Convert.ToString(objdb.GetValue(reader1, "description")));
                    collection.Add("type", Convert.ToString(objdb.GetValue(reader1, "type")));
                    collection.Add("eventid", Convert.ToString(objdb.GetValue(reader1, "eventid")));
                    collection.Add("sponsor_file", Convert.ToString(objdb.GetValue(reader1, "sponsor_file")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetSponsorDetail", x.Message);
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

        public static int GetEventSponsorCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(s.id) as maxcount from [event_sponsors] s inner join [event] e on s.eventid = e.id where s.active = 1 and e.active = 1";
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
                objdb.Write_log_file("GetEventSponsorCount", x.Message);
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

        public static DataSet GetEventSponsorDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;
            string query = "select s.* from [event_sponsors] s inner join [event] e on s.eventid = e.id where s.active = 1 and e.active = 1 ";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by s.[name] offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static bool DeleteEventSponsor(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update [event_sponsors] set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string SponsorSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["eventid"] != null && xiCollection["eventid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(s.eventid={0})", Utils.ConvertToDBString(xiCollection["eventid"], Utils.DataType.Integer)));
            }

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(s.[name] like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            return builder.ToString();
        }

        public static DataTable GetPageSponsorDetails(object xiPageId)
        {
            string query = @"select a.[name],a.[description],a.sponsor_file from [event_sponsors] a
where active = 1 and id in (select[value] from[event_brochure_pages] bp CROSS APPLY STRING_SPLIT_2(bp.content_data, ',') left join[event_sponsors] a on a.id = value where bp.id = " + Utils.ConvertToDBString(xiPageId, Utils.DataType.Integer) + ")";
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dt = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dt;
        }
        #endregion
    }
}
