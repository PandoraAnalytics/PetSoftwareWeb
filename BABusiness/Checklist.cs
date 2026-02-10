using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BABusiness
{
    public class Checklist
    {
        public int Add(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string categoryquery = @"if not exists(select 1 from checklist_category where name = @category and active = 1) insert into checklist_category(name,active) values(@category, 1)";
            Parameter param1 = new Parameter("category", xiCollection["category"]);
            objdb.ExecuteNonQuery(objdb.con, categoryquery, new Parameter[] { param1 });

            string query = @"insert into checklist(name,category,responsetype,createdby,active) values(@name, (select top 1 id from checklist_category where [name] = @category and active = 1), @responsetype,@userid, 1)";
            Parameter param2 = new Parameter("name", xiCollection["name"]);
            Parameter param3 = new Parameter("responsetype", xiCollection["responsetype"], DbType.Int32);
            Parameter param4 = new Parameter("userid", xiCollection["userid"], DbType.Int32);

            string query2 = "select scope_identity()";

            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return value;
        }

        public bool Update(NameValueCollection xiCollection, object xiChecklistId)
        {
            if (xiCollection == null) return false;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string categoryquery = @"if not exists(select 1 from checklist_category where name = @category and active = 1) insert into checklist_category(name,active) values(@category, 1)";
            Parameter param1 = new Parameter("category", xiCollection["category"]);
            objdb.ExecuteNonQuery(objdb.con, categoryquery, new Parameter[] { param1 });

            string query = @"update checklist set name=@name,responsetype=@responsetype,category=(select top 1 id from checklist_category where [name]=@category and active=1) where id=@id and active=1";
            Parameter param2 = new Parameter("id", xiChecklistId, DbType.Int32);
            Parameter param3 = new Parameter("name", xiCollection["name"]);
            Parameter param4 = new Parameter("responsetype", xiCollection["responsetype"], DbType.Int32);

            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetChecklist(object xiChecklistId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select c.*, cat.name as categoryname from checklist c left join checklist_category cat on cat.id = c.category and cat.active =1 where c.id = " + Utils.ConvertToDBString(xiChecklistId, Utils.DataType.Integer) + " and c.active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();

                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("category", Convert.ToString(objdb.GetValue(reader1, "category")));
                    collection.Add("responsetype", Convert.ToString(objdb.GetValue(reader1, "responsetype")));
                    collection.Add("categoryname", Convert.ToString(objdb.GetValue(reader1, "categoryname")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetChecklist", x.Message);
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
            if (!string.IsNullOrEmpty(xiCollection["name"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("c.[name] like {0}", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            if (BusinessBase.ConvertToInteger(xiCollection["notinchecklist_animalid"]) > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("c.id not in (select checklistid from animal_checklist where animalid = {0})", Utils.ConvertToDBString(xiCollection["notinchecklist_animalid"], Utils.DataType.Integer)));
            }

            if (BusinessBase.ConvertToInteger(xiCollection["categoryid"]) > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.category={0})", Utils.ConvertToDBString(xiCollection["categoryid"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static string SearchUsers(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;
            if (!string.IsNullOrEmpty(xiCollection["checklistname"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("c.[name] like {0}", Utils.ConvertToDBString("%" + xiCollection["checklistname"] + "%", Utils.DataType.String)));
            }

            if (!string.IsNullOrEmpty(xiCollection["animalname"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("a.[name] like {0}", Utils.ConvertToDBString("%" + xiCollection["animalname"] + "%", Utils.DataType.String)));
            }

            if (BusinessBase.ConvertToInteger(xiCollection["animalid"]) > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.id={0})", Utils.ConvertToDBString(xiCollection["animalid"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static int GetChecklistCount(string xiFilter, object xiUserId)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from checklist c left join checklist_category cat on cat.id = c.category and cat.active =1 where c.active = 1 and c.createdby = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetChecklistCount", x.Message);
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

        public static DataSet GetAllChecklists(int xiPage, string xiFilter, object xiUserId)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select c.*,(select count(*) from [checklist_customfields] where [checklistid] = c.id) as checkpointcount, cat.name as categoryname from checklist c left join checklist_category cat on cat.id = c.category and cat.active =1 where c.active = 1 and c.createdby = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.[name] offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static bool DeleteChecklist(int xiChecklistId)
        {
            string query = "update checklist set active = 0 where id = {0}";
            query = string.Format(query, Utils.ConvertToDBString(xiChecklistId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool AssignOtherFields(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = @"if not exists(select 1 from checklist_customfields where checklistid=@checklistid and fieldid=@fieldid) insert into checklist_customfields(checklistid,fieldid) values(@checklistid,@fieldid)";
            Parameter param1 = new Parameter("checklistid", xiCollection["checklistid"], DbType.Int32);
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

            string query = @"delete from checklist_customfields where checklistid=@checklistid and fieldid=@fieldid";
            Parameter param1 = new Parameter("checklistid", xiCollection["checklistid"], DbType.Int32);
            Parameter param2 = new Parameter("fieldid", xiCollection["fieldid"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetAssignedOtherFields(object xiChecklistId)
        {
            string query = @"select cf.checklistid, c.*
from checklist_customfields cf inner join animal_customfields c on c.id = cf.fieldid
where c.active = 1 and c.breedtype='checklist' and cf.checklistid = " + Utils.ConvertToDBString(xiChecklistId, Utils.DataType.Integer) + " order by c.sortorder,c.title";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public int AddSchedule(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = @"if not exists(select 1 from checklist_schedule where checklistid=@checklistid and active=1) 
insert into checklist_schedule(checklistid,repeattype,repeatval,endson,active) values(@checklistid,@repeattype,@repeatval,@endson,1)";
            Parameter param1 = new Parameter("checklistid", xiCollection["checklistid"], DbType.Int32);
            Parameter param2 = new Parameter("repeattype", xiCollection["repeattype"], DbType.Int32);
            Parameter param3 = new Parameter("repeatval", xiCollection["repeatval"]);
            Parameter param4 = new Parameter("endson", xiCollection["endson"], DbType.Date);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int scheduleid = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return scheduleid;
        }

        public bool UpdateSchedule(NameValueCollection xiCollection, object xiScheduleId)
        {
            if (xiCollection == null) return false;

            string query = "update checklist_schedule set repeattype=@repeattype, repeatval=@repeatval, endson=@endson where id=@id and active=1";
            Parameter param1 = new Parameter("id", xiScheduleId, DbType.Int32);
            Parameter param2 = new Parameter("repeattype", xiCollection["repeattype"], DbType.Int32);
            Parameter param3 = new Parameter("repeatval", xiCollection["repeatval"]);
            Parameter param4 = new Parameter("endson", xiCollection["endson"], DbType.Date);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool AddScheduleDates(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "insert into checklist_schedule_dates(scheduleid,startdate,enddate,seqno) values(@scheduleid,@startdate,@enddate, (select count(*)+1 from checklist_schedule_dates where scheduleid = @scheduleid))";

            Parameter param1 = new Parameter("scheduleid", xiCollection["scheduleid"], DbType.Int32);
            Parameter param2 = new Parameter("startdate", xiCollection["startdate"], DbType.DateTime);
            Parameter param3 = new Parameter("enddate", xiCollection["startdate"], DbType.DateTime); // startdate and enddate is same

            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetScheduleFromChecklist(object xiChecklistId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from checklist_schedule where checklistid = " + Utils.ConvertToDBString(xiChecklistId, Utils.DataType.Integer) + " and active=1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("repeattype", Convert.ToString(objdb.GetValue(reader1, "repeattype")));
                    collection.Add("repeatval", Convert.ToString(objdb.GetValue(reader1, "repeatval")));
                    collection.Add("endson", Convert.ToString(objdb.GetValue(reader1, "endson")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetScheduleFromChecklist", x.Message);
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

        public static NameValueCollection GetSchedule(object xiScheduleDateId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select csd.scheduleid, csd.startdate, cs.checklistid, c.name as checklistname
from checklist_schedule_dates csd 
inner join checklist_schedule cs on cs.id = csd.scheduleid and cs.active =1 
inner join checklist c on c.id = cs.checklistid and c.active=1 
where csd.id = " + Utils.ConvertToDBString(xiScheduleDateId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("scheduleid", Convert.ToString(objdb.GetValue(reader1, "scheduleid")));
                    collection.Add("startdate", Convert.ToString(objdb.GetValue(reader1, "startdate")));
                    collection.Add("checklistid", Convert.ToString(objdb.GetValue(reader1, "checklistid")));
                    collection.Add("checklistname", Convert.ToString(objdb.GetValue(reader1, "checklistname")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetSchedule", x.Message);
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

        public static NameValueCollection GetSchedule(object xiScheduleDateId, object xiUserId, object xiAnimalId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select csd.scheduleid, csd.startdate, cs.checklistid, c.name as checklistname, cr.id as responseid, cr.updateddate, cr.isdraft
from checklist_schedule_dates csd 
inner join checklist_schedule cs on cs.id = csd.scheduleid and cs.active =1 
inner join checklist c on c.id = cs.checklistid and c.active=1 
left join checklistresponse cr on cr.checklistid = cs.checklistid and cr.scheduleid = csd.id and cr.animalid= " + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer) + " and cr.userid= " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + @" and cr.active=1
where csd.id = " + Utils.ConvertToDBString(xiScheduleDateId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("scheduleid", Convert.ToString(objdb.GetValue(reader1, "scheduleid")));
                    collection.Add("startdate", Convert.ToString(objdb.GetValue(reader1, "startdate")));
                    collection.Add("checklistid", Convert.ToString(objdb.GetValue(reader1, "checklistid")));
                    collection.Add("checklistname", Convert.ToString(objdb.GetValue(reader1, "checklistname")));
                    collection.Add("responseid", Convert.ToString(objdb.GetValue(reader1, "responseid")));
                    collection.Add("updateddate", Convert.ToString(objdb.GetValue(reader1, "updateddate")));
                    collection.Add("isdraft", Convert.ToString(objdb.GetValue(reader1, "isdraft")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetSchedule", x.Message);
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

        public bool DeleteScheduleDates(int xiScheduleId)
        {
            string query = "delete from checklist_schedule_dates where scheduleid = " + Utils.ConvertToDBString(xiScheduleId, Utils.DataType.Integer) + " and cast(startdate as date) > cast(getutcdate() as date)";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataSet GetChecklistSchedules(object xiCheckListId, object xiAnimalId)
        {
            string query = @"select csd.id, cs.id as scheduleid, cs.checklistid, csd.startdate, c.name as checklistname,
(select count(au.id) from checklist_user au inner join [user] s on au.userid=s.id and s.active = 1 where au.checklistid = c.id) as assigned_usercount,
(select count(cr.id) from checklistresponse cr inner join [user] s on cr.userid=s.id and cr.animalid = " + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer) + @" and cr.scheduleid = csd.id and s.active = 1 where cr.checklistid = c.id and cr.isdraft=0 and cr.active=1) as responsecount 
from checklist_schedule cs inner join checklist_schedule_dates csd on csd.scheduleid = cs.id 
inner join checklist c on c.id = cs.checklistid and c.active=1
where cs.checklistid = " + Utils.ConvertToDBString(xiCheckListId, Utils.DataType.Integer) + " and cs.active =1 order by csd.startdate";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_date", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["startdate"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_date"] = tempDate1.ToString(BusinessBase.DateFormat);
                    }
                }
            }

            return ds;
        }

        public static DataSet GetChecklistScheduleResponses(object xiScheduleDateId, object xiAnimalId)
        {
            string query = @"select csd.id, s.fname as fname,s.lname as lname, cr.id as responseid, cr.updateddate, cr.userid
from checklist_schedule_dates csd inner join checklist_schedule cs on cs.id = csd.scheduleid and cs.active =1
inner join checklist c on c.id = cs.checklistid and c.active=1 
inner join checklist_user cu on cu.checklistid = cs.checklistid
inner join [user] s on cu.userid=s.id and s.active = 1
left join checklistresponse cr on cr.checklistid = cs.checklistid and cr.animalid = " + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer) + @" and cr.scheduleid = " + Utils.ConvertToDBString(xiScheduleDateId, Utils.DataType.Integer) + @" and cr.userid=cu.userid and cr.active=1 and cr.isdraft=0
where csd.id = " + Utils.ConvertToDBString(xiScheduleDateId, Utils.DataType.Integer) + " order by s.fname,s.lname";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_date", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["updateddate"] != DBNull.Value)
                    {
                        DateTime tempDate1 = Convert.ToDateTime(row["updateddate"]);
                        if (tempDate1 != DateTime.MinValue)
                        {
                            row["procesed_date"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                        }
                    }
                }
            }

            return ds;
        }

        public static DataTable GetChecklistUsers(object xiChecklistId)
        {
            string query = @"select au.id,(s.fname + ' ' + s.lname) as name,s.email as email,s.phone as phone from checklist_user au inner join [user] s on au.userid=s.id and s.active = 1 where au.checklistid =" + Utils.ConvertToDBString(xiChecklistId, Utils.DataType.Integer) + " order by s.fname,s.lname";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static bool RemoveChecklistUser(object xiChecklistUserId)
        {
            string query = "delete from checklist_user where id = " + Utils.ConvertToDBString(xiChecklistUserId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public int AddUsersToChecklist(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "if not exists(select 1 from checklist_user where checklistid=@checklistid and userid=@userid) insert into checklist_user(checklistid,userid,active) values(@checklistid, @userid,1)";

            Parameter param1 = new Parameter("checklistid", xiCollection["checklistid"], DbType.Int32);
            Parameter param2 = new Parameter("userid", xiCollection["userid"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public static DataSet GetAllChecklistsForUsers(string xiFilter, string xiUserId)
        {
            string query = @"select top 50 c.id as checklistid, c.name as checklistname, csd.id as scheduleid, csd.startdate as scheduledate, cu.userid,
cr.id as responseid, cr.updateddate as responsedate, cr.isdraft,cat.name as categoryname, a.id as animalid, a.name, a.typename as animaltype, a.profilepic_file, a.breedimage,a.animalcategory
from checklist_user cu 
inner join checklist c on cu.checklistid = c.id
inner join checklist_schedule cs on cs.checklistid = c.id and cs.active =1 
inner join checklist_schedule_dates csd on csd.scheduleid = cs.id 
inner join animal_checklist ac on ac.checklistid = c.id
inner join view_animal a on a.id = ac.animalid and a.active =1
left join checklistresponse cr on cr.scheduleid = csd.id and cr.animalid = a.id and cr.userid = (case c.responsetype 
when 0 then (select top 1 userid from checklistresponse where scheduleid = csd.id and animalid =a.id order by updateddate desc) else cu.userid end)  and cr.active=1
left join checklist_category cat on cat.id = c.category and cat.active =1 
where cu.userid = {0} and c.active = 1 and cast(csd.startdate as date) <= {1}";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by csd.startdate desc";

            query = string.Format(query, Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer), Utils.ConvertToDBString(BusinessBase.Now, Utils.DataType.Date));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_scheduledate", typeof(string));
                ds.Tables[0].Columns.Add("procesed_responsedate", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["scheduledate"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_scheduledate"] = tempDate1.ToString(BusinessBase.DateFormat);
                    }

                    if (row["responsedate"] != DBNull.Value)
                    {
                        tempDate1 = Convert.ToDateTime(row["responsedate"]);
                        if (tempDate1 != DateTime.MinValue)
                        {
                            row["procesed_responsedate"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                        }
                    }
                }
            }

            return ds;
        }

        public bool SaveComments(NameValueCollection xiCollection, object xiResponseId)
        {
            if (xiCollection == null) return false;

            string query = "insert into checklistresponsecomments(responseid,comments,files,createdby,createddate,active) values(@responseid,@comments,@files,@userid,getutcdate(),1)";

            Parameter param1 = new Parameter("responseid", xiResponseId, DbType.Int32);
            Parameter param2 = new Parameter("comments", xiCollection["comments"]);
            Parameter param3 = new Parameter("files", xiCollection["files"]);
            Parameter param4 = new Parameter("userid", xiCollection["userid"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetResponseComments(object xiResponseId)
        {
            string query = @"select rcom.id, rcom.comments, rcom.files, rcom.createdby, rcom.createddate,  (s.fname + ' ' + s.lname) as username 
from checklistresponsecomments rcom inner join [user] s on rcom.createdby=s.id and s.active = 1 where rcom.responseid =" + Utils.ConvertToDBString(xiResponseId, Utils.DataType.Integer) + " and rcom.active = 1 order by rcom.createddate";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            if (dataTable != null)
            {
                dataTable.Columns.Add("procesed_createddate", typeof(string));

                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["createddate"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_createddate"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                    }
                }
            }

            return dataTable;
        }

        #region Checklist Cateogry

        public bool AddChecklistCategory(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into checklist_category(name,active) values(@name,1)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateChecklistCategory(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update checklist_category set name=@name where id=@id and active = 1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetChecklistCategory(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name from checklist_category where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

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
                objdb.Write_log_file("GetChecklistCategory", x.Message);
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

        public static bool DeleteChecklistCategory(int xiId)
        {
            string query = string.Format("update checklist_category set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetChecklistCategory()
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from checklist_category where active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataSet GetChecklistCategories(string xiFilter)
        {
            string query = "select id,name from checklist_category where active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        #endregion

        #region Checklist Response

        public int InsertResponse(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "insert into checklistresponse(checklistid,scheduleid,userid,animalid,isdraft,createddate,updateddate,active) values(@checklistid,@scheduleid,@userid,@animalid,@isdraft,getutcdate(),getutcdate(),1)";

            Parameter param1 = new Parameter("checklistid", xiCollection["checklistid"], DbType.Int32);
            Parameter param2 = new Parameter("scheduleid", xiCollection["scheduleid"], DbType.Int32);
            Parameter param3 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param4 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
            Parameter param5 = new Parameter("isdraft", xiCollection["isdraft"], DbType.Int16);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5 });
            objdb.Disconnectdb();

            if (value > 0)
            {
                query = string.Format(@"insert into checklistresponsequestion(checklistresponseid,fieldid) 
 select {0}, fieldid from checklist_customfields where checklistid = {1}", value, Utils.ConvertToDBString(xiCollection["checklistid"], Utils.DataType.Integer));

                objdb.Connectdb();
                objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();
            }

            return value;
        }

        public bool UpdateResponse(NameValueCollection xiCollection, object xiResponseId)
        {
            if (xiCollection == null) return false;

            string query = "update checklistresponse set isdraft=@isdraft,userid=@userid,updateddate=getutcdate() where id=@responseid and active = 1";

            Parameter param1 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param2 = new Parameter("isdraft", xiCollection["isdraft"], DbType.Int16);
            Parameter param3 = new Parameter("responseid", xiResponseId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public void SaveResponseQuestion(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return;

            Parameter[] param = new Parameter[3];
            param[0] = new Parameter("rid", xiCollection["responseid"], DbType.Int32);
            param[1] = new Parameter("fid", xiCollection["fieldid"], DbType.Int32);
            param[2] = new Parameter("ans", xiCollection["fieldvalue"], DbType.String);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            objdb.ExecuteStoreProc(objdb.con, "checklist_saveresponsequestion", param);
            objdb.Disconnectdb();
        }

        public static NameValueCollection GetCheckListResponse(object xiResponseId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select cr.*, c.name as checklistname, (u.fname + ' ' + u.lname) as username from checklistresponse cr inner join checklist c on cr.checklistid = c.id and c.active = 1
inner join [user] u on u.id = cr.userid and u.active=1 inner join view_animal a on a.id = cr.animalid and a.active =1
where cr.id =" + Utils.ConvertToDBString(xiResponseId, Utils.DataType.Integer) + " and cr.active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();

                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("checklistid", Convert.ToString(objdb.GetValue(reader1, "checklistid")));
                    collection.Add("animalid", Convert.ToString(objdb.GetValue(reader1, "animalid")));
                    collection.Add("checklistname", Convert.ToString(objdb.GetValue(reader1, "checklistname")));
                    collection.Add("userid", Convert.ToString(objdb.GetValue(reader1, "userid")));
                    collection.Add("username", Convert.ToString(objdb.GetValue(reader1, "username")));
                    collection.Add("isdraft", Convert.ToString(objdb.GetValue(reader1, "isdraft")));
                    collection.Add("updateddate", Convert.ToString(objdb.GetValue(reader1, "updateddate")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCheckListResponse", x.Message);
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

        public static DataTable GetCheckListResponseAnswers(object xiResponseId)
        {
            NameValueCollection resCollection = Checklist.GetCheckListResponse(xiResponseId);
            if (resCollection == null) return null;

            string query = @"select cf.checklistid, c.*, cr.id as responseid, crq.fieldvalue,(select stuff(( 
select ', ' + co.optiontext from animal_customfieldsoption co where co.id in
(select value from checklistresponsequestion crq1
CROSS APPLY STRING_SPLIT(SUBSTRING (crq1.fieldvalue ,0 ,LEN(crq1.fieldvalue)+100), ',') where crq1.id = crq.id and crq.fieldid in (select id from animal_customfields where [type] in (3,4,5)))
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as optiontext
from checklist_customfields cf
inner join animal_customfields c on c.id = cf.fieldid and c.active=1
left join checklistresponse cr on cr.active=1 and cr.checklistid = cf.checklistid and cr.id = " + Utils.ConvertToDBString(xiResponseId, Utils.DataType.Integer) + @"
left join checklistresponsequestion crq on crq.checklistresponseid = cr.id and crq.fieldid = cf.fieldid
where c.active = 1 and cf.checklistid = " + Utils.ConvertToDBString(resCollection["checklistid"], Utils.DataType.Integer) + " order by c.sortorder,c.title";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetCheckListResponseAnswers(object xiScheduledDateId, object xiUserId, object xiAnimalId)
        {
            NameValueCollection collection = Checklist.GetSchedule(xiScheduledDateId);
            if (collection == null) return null;

            string query = @"select cf.checklistid, c.*, cr.id as responseid, crq.fieldvalue,(select stuff(( 
select ', ' + co.optiontext from animal_customfieldsoption co where co.id in
(select value from checklistresponsequestion crq1
CROSS APPLY STRING_SPLIT(SUBSTRING (crq1.fieldvalue ,0 ,LEN(crq1.fieldvalue)+100), ',') where crq1.id = crq.id and crq.fieldid in (select id from animal_customfields where [type] in (3,4,5)))
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as optiontext
from checklist_customfields cf
inner join animal_customfields c on c.id = cf.fieldid and c.active=1
left join checklistresponse cr on cr.active=1 and cr.checklistid = cf.checklistid and cr.animalid = " + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer) + @"
and cr.userid = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and cr.scheduleid =  " + Utils.ConvertToDBString(xiScheduledDateId, Utils.DataType.Integer) + @"
left join checklistresponsequestion crq on crq.checklistresponseid = cr.id and crq.fieldid = cf.fieldid
where c.active = 1 and cf.checklistid = " + Utils.ConvertToDBString(collection["checklistid"], Utils.DataType.Integer) + " order by c.sortorder,c.title";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        #endregion

        // new code

        public static NameValueCollection GetCheckListDetailsFromScheduleId(string xiScheduleId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select cs.checklistid, csd.startdate, csd.enddate, c.[name] as checklistname from checklist_schedule_dates csd 
inner join checklist_schedule cs on csd.chklist_schedule_id = cs.id inner join checklist c on c.id = cs.checklistid and c.active = 1
where csd.id  = " + Utils.ConvertToDBString(xiScheduleId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("checklistid", Convert.ToString(objdb.GetValue(reader1, "checklistid")));
                    collection.Add("startdate", Convert.ToString(objdb.GetValue(reader1, "startdate")));
                    collection.Add("enddate", Convert.ToString(objdb.GetValue(reader1, "enddate")));
                    collection.Add("checklistname", Convert.ToString(objdb.GetValue(reader1, "checklistname")));
                    //collection.Add("isthirdpartyapproval", Convert.ToString(objdb.GetValue(reader1, "isthirdpartyapproval")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCheckListDetailsFromScheduleId", x.Message);
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

        public static DataTable GetCheckListQuestions(int xiChecklistId)
        {
            return GetCheckListQuestions(xiChecklistId, string.Empty);
        }

        public static DataTable GetCheckListQuestions(int xiChecklistId, string xiScheduleIds)
        {
            string checkListId = Utils.ConvertToDBString(xiChecklistId, Utils.DataType.Integer);



            string query = @"select q.*,
(select count(*)  from checklistresponse cr inner join checklistresponsequestion res on cr.id = res.checklistresponseid 
where cr.checklistid = {0} and fieldid = q.id and cr.isdraft = 0 and cr.scheduleid > 0 {1} group by fieldid,cr.checklistid ) as rescount, 
(select count(*)  from checklistresponse cr inner join checklistresponsequestion res on cr.id = res.checklistresponseid 
where cr.checklistid = {0} and fieldid = q.id and cr.isdraft = 0  and cr.scheduleid > 0 {1} group by fieldid,cr.checklistid ) as additionalcommentscount, 
(select count(*) from checklistresponse cr where checklistid = {0} and isdraft = 0 and cr.scheduleid > 0 {1}) as totcount 
from checklist_customfields cq inner join animal_customfields q on cq.fieldid = q.id and q.breedtype='checklist'
where cq.checklistid = {0} and q.active = 1 order by q.title";

            if (!string.IsNullOrEmpty(xiScheduleIds)) xiScheduleIds = "and cr.scheduleid in (@" + string.Join(",",
    xiScheduleIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Utils.ConvertToDBString(x, Utils.DataType.Integer))) + ")";

            query = string.Format(query, checkListId, xiScheduleIds);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetCheckListQuestionAnswersBySchedule(string xiCheckListId, string xiScheduleId, string xiPeopleId)
        {
            NameValueCollection checklistcollection = Checklist.GetChecklist(xiCheckListId);
            if (checklistcollection == null) return null;

            string peopleId = (checklistcollection["responsetype"] == "1") ?
                "(select top 1 userid from checklistresponse where scheduleid = " + Utils.ConvertToDBString(xiScheduleId, Utils.DataType.Integer) + " order by updateddate desc)" :
                Utils.ConvertToDBString(xiPeopleId, Utils.DataType.Integer);

            string query = @"select q.*, cat.[name] as categoryname, subcat.[name] as subcategoryname,  lo.[name] as locationname, crq.optioncomment, crq.optionid, qo.optiontext,
crq.id as queresponseid, crq.comments as additionalcomment, 
stuff((select ','+f.[file_name] from checklistresponsequestion_files f where f.checklistresponsequestionid = crq.id and f.active = 1 for xml path(''), type).value('.','nvarchar(max)'),1,1,'') as files 
from checklistquestion cq 
inner join checklist c on c.id = cq.checklistid and c.active = 1
inner join question q on cq.questionid = q.id and q.active = 1 
left join checklist_category cat on cat.id = q.categoryid left join checklist_subcategory subcat on subcat.id = q.subcategoryid
left join locations lo on lo.id = q.locationid and lo.active = 1
left join checklistresponse cr on cr.checklistid = cq.checklistid and cr.workforceid = {0} and cr.checklistscheduleid = {1}
left join checklistresponsequestion crq on crq.checklistresponseid = cr.id and crq.questionid = cq.questionid
left join questionoption qo on qo.questionid = cq.questionid and qo.id = crq.optionid
where cq.checklistid = {2} order by cq.sortorder, q.title";

            query = string.Format(query, peopleId, Utils.ConvertToDBString(xiScheduleId, Utils.DataType.Integer), Utils.ConvertToDBString(xiCheckListId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }
    }
}
