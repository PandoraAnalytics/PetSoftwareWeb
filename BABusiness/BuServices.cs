using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BABusiness
{
    public class BuServices : BusinessBase
    {
        public enum Status
        {
            SERVICEADD = 1,
            SERVICEEDIT = 2,
            SERVICEDELETE = 3,
            SERVICEIMAGEADD = 4,
            SERVICEIMAGEDELETE = 5,
        }

        public static string GetStatusLogText(object xiStatus, object xiAdditionMessage)
        {
            string statusText = string.Empty;
            try
            {
                switch (Convert.ToInt32(xiStatus))
                {
                    case (int)Status.SERVICEADD:
                        statusText = "The service has been added.";
                        break;

                    case (int)Status.SERVICEEDIT:
                        statusText = "The service has been edited.";
                        break;

                    case (int)Status.SERVICEDELETE:
                        statusText = "The service has been deleted.";
                        break;

                    case (int)Status.SERVICEIMAGEADD:
                        statusText = "The service image has been added.";
                        break;

                    case (int)Status.SERVICEIMAGEDELETE:
                        statusText = "The service image has been deleted.";
                        break;

                    default:
                        statusText = "-";
                        break;
                }
            }
            catch { }

            return statusText;
        }

        #region Service

        public static int AddService(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [bu_services]([name],[description],[services_code],[type_id],[cost],[final_cost],[status],[created],[createdby],[updated],[updatedby],[bu_id],[profileimage],[active],[taxid]) values(@name,@description,@services_code,@type_id,@cost,@final_cost,@status,getutcdate(),@createdby,getutcdate(),@updatedby,@bu_id,@profileimage,@active,@taxid)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("services_code", xiCollection["services_code"]);
            Parameter param3 = new Parameter("cost", xiCollection["cost"], DbType.Int32);
            Parameter param4 = new Parameter("final_cost", xiCollection["final_cost"], DbType.Int32);
            Parameter param5 = new Parameter("type_id", xiCollection["type_id"], DbType.Int32);
            Parameter param6 = new Parameter("description", xiCollection["description"]);
            Parameter param7 = new Parameter("createdby", xiCollection["userid"], DbType.Int32);
            Parameter param8 = new Parameter("updatedby", xiCollection["userid"], DbType.Int32);
            Parameter param9 = new Parameter("active", "1");
            Parameter param10 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param11 = new Parameter("bu_id", xiCollection["companyid"], DbType.Int32);
            Parameter param12 = new Parameter("profileimage", xiCollection["profileimage"]);
            Parameter param13 = new Parameter("taxid", xiCollection["taxid"], DbType.Int32);


            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateService(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [bu_services] set name=@name,cost=@cost,final_cost=@final_cost,type_id=@type_id,description=@description,updated=getutcdate(),status=@status,profileimage=@profileimage,taxid=@taxid where id=@id";
            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("cost", xiCollection["cost"], DbType.Int32);
            Parameter param3 = new Parameter("final_cost", xiCollection["final_cost"], DbType.Int32);
            Parameter param4 = new Parameter("type_id", xiCollection["type_id"], DbType.Int32);
            Parameter param5 = new Parameter("description", xiCollection["description"]);
            Parameter param6 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param7 = new Parameter("profileimage", xiCollection["profileimage"]);
            Parameter param8 = new Parameter("taxid", xiCollection["taxid"], DbType.Int32);
            Parameter param9 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string DeleteServices(object xiId, string xiUserId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string query = @"select count(a.id) as maxcount from bu_service_images a 
   inner join bu_services c on a.serviceid = c.id where a.serviceid = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("DeleteServices", x.Message);
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
                query = string.Format("update bu_services set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

                objdb.Connectdb();
                int value = objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();

                returnValue = (value > 0) ? "Deleted Successfully." : "false";
                //log for delete
                NameValueCollection collection = new NameValueCollection();
                collection["service_id"] = xiId.ToString();
                collection["user_id"] = xiUserId.ToString();
                collection["message_id"] = (int)BuServices.Status.SERVICEDELETE + "";
                collection["old_entry"] = string.Empty;
                collection["new_entry"] = string.Empty;
                collection["comment"] = "Service deleted.";
                BuServices.AddServiceLog(collection);
            }
            else
            {
                returnValue = "You can not delete this item because its already used.";
            }
            objdb.Disconnectdb();

            return returnValue;
        }

        public static int GetServicesCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from [bu_services] c inner join bu_services_type st on st.id= c.type_id inner join bu_tax t on t.id= c.taxid where c.active = 1";
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
                objdb.Write_log_file("GetServicesCount", x.Message);
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

        public static DataSet GetServicesDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select c.*,st.[name] as typename,t.[name] as taxname,t.[percentage] as taxpercentage from [bu_services] c inner join bu_services_type st on st.id= c.type_id inner join bu_tax t on t.id= c.taxid where c.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.name offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_cost", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string tempCost = Convert.ToDecimal(row["cost"]).ToString("0.00").Replace(",", ".");

                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";

                    }
                    row["processed_cost"] = tempCost;
                }
            }
            return ds;
        }

        public static int GetServiceForComboCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from [bu_services] c inner join bu_services_type st on st.id= c.type_id inner join bu_tax t on t.id= c.taxid where c.active = 1";
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
                objdb.Write_log_file("GetServiceForComboCount", x.Message);
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
            int divide = (int)(totalCount / 10);
            int mod = totalCount % 10;
            return (mod == 0) ? divide : divide + 1;
        }

        public static DataSet GetServicesForComboDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select c.*,st.[name] as typename,t.[name] as taxname,t.[percentage] as taxpercentage from [bu_services] c inner join bu_services_type st on st.id= c.type_id inner join bu_tax t on t.id= c.taxid where c.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.name offset " + (xiPage * 10) + " rows fetch next " + 10 + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds; 
        }

        public static NameValueCollection GetServiceDetail(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;

            string query = "select c.*,st.[name] as typename,t.[name] as taxname,t.[percentage] as taxpercentage from bu_services c inner join bu_services_type st on st.id= c.type_id inner join bu_tax t on t.id= c.taxid where c.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and c.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and c.active = 1";
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
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("services_code", Convert.ToString(objdb.GetValue(reader1, "services_code")));
                    collection.Add("type_id", Convert.ToString(objdb.GetValue(reader1, "type_id")));
                    collection.Add("typename", Convert.ToString(objdb.GetValue(reader1, "typename")));
                    collection.Add("cost", Convert.ToString(objdb.GetValue(reader1, "cost")));
                    collection.Add("final_cost", Convert.ToString(objdb.GetValue(reader1, "final_cost")));
                    collection.Add("description", Convert.ToString(objdb.GetValue(reader1, "description")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                    collection.Add("updatedby", Convert.ToString(objdb.GetValue(reader1, "updatedby")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("profileimage", Convert.ToString(objdb.GetValue(reader1, "profileimage")));
                    collection.Add("bu_id", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                    collection.Add("taxid", Convert.ToString(objdb.GetValue(reader1, "taxid")));
                    collection.Add("taxname", Convert.ToString(objdb.GetValue(reader1, "taxname")));
                    collection.Add("taxpercentage", Convert.ToString(objdb.GetValue(reader1, "taxpercentage")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetServiceDetail", x.Message);
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
                builder.Append(string.Format("(c.name like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["type_id"] != null && xiCollection["type_id"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.type_id = {0})", Utils.ConvertToDBString(xiCollection["type_id"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static bool DeleteTags(object xiId)
        {
            DBClass objdb = new DBClass();
            string query = string.Format("delete from  bu_service_tags where service_id=@service_id");
            Parameter param1 = new Parameter("service_id", xiId, DbType.Int32);

            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int AddServiceTag(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [bu_service_tags]([name],[service_id]) values(@name,@service_id)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("service_id", xiCollection["service_id"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public static DataTable GetServiceTagDetails(object xiServiceId)
        {
            int service = BusinessBase.ConvertToInteger(xiServiceId);
            string query = "select c.* from bu_service_tags c where c.service_id=" + Utils.ConvertToDBString(xiServiceId, Utils.DataType.Integer);
            // string query = string.Format(@"select c.* from bu_service_tags c where c.service_id=", service);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static bool AddServiceLog(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into bu_service_log(service_id, user_id, [datetime],message_id, old_entry, new_entry, comment) values(@service_id,@user_id,getutcdate(),@message_id,@old_entry,@new_entry,@comment)";

            Parameter param1 = new Parameter("service_id", xiCollection["service_id"], DbType.Int32);
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

        #region Service_Gallery

        public static bool AddServiceGallery(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [bu_service_images]([serviceid],[title],[gallery_file],[file_type],[active],[submitdate]) values(@serviceid,@title,@file_name,@file_type,1,getutcdate())";
            Parameter param1 = new Parameter("serviceid", xiCollection["serviceid"], DbType.Int32);
            Parameter param2 = new Parameter("file_name", xiCollection["file_name"]);
            Parameter param3 = new Parameter("title", xiCollection["title"]);
            Parameter param4 = new Parameter("file_type", xiCollection["file_type"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            //add log
            NameValueCollection logCollection = new NameValueCollection();
            logCollection["service_id"] = xiCollection["serviceid"];
            logCollection["user_id"] = xiCollection["userid"];
            logCollection["message_id"] = (int)BuServices.Status.SERVICEIMAGEADD + "";
            logCollection["old_entry"] = string.Empty;
            logCollection["new_entry"] = string.Empty;
            logCollection["comment"] = xiCollection["file_name"];
            BuServices.AddServiceLog(logCollection);

            return (value > 0);
        }

        public static DataSet GetServicePhotos(object xiServiceId)
        {
            string query = "select * from bu_service_images where active=1 and serviceid=" + Utils.ConvertToDBString(xiServiceId, Utils.DataType.Integer) + " order by submitdate desc";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static NameValueCollection GetGallaryDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from bu_service_images  where active=1 and serviceid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

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
                objdb.Write_log_file("GetGallaryDetail", x.Message);
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

        public static bool DeleteSeviceGalleryPhoto(string xiFileName, NameValueCollection xiCollection)
        {
            string query = "delete from [bu_service_images] where [gallery_file]=@file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();
            if (xiCollection != null)
            {
                // add log here
                NameValueCollection logCollection = new NameValueCollection();
                logCollection["service_id"] = xiCollection["serviceid"];
                logCollection["user_id"] = xiCollection["userid"];
                logCollection["message_id"] = (int)BuServices.Status.SERVICEIMAGEDELETE + "";
                logCollection["old_entry"] = string.Empty;
                logCollection["new_entry"] = string.Empty;
                logCollection["comment"] = xiFileName;
                BuServices.AddServiceLog(logCollection);
            }
            return (value > 0);

        }

        #endregion

        #region Service Type Class

        public static int AddServiceType(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "insert into bu_services_type(name,bu_id,active)values(@name,@bu_id,1)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("bu_id", xiCollection["companyid"]);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateServiceType(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_services_type set name=@name where id=@id and active = 1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetServiceType(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_services_type where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

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
                objdb.Write_log_file("GetServiceType", x.Message);
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

        public static NameValueCollection GetServiceTypeClassName(object xiName)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_services_type where active=1 and name=" + Utils.ConvertToDBString(xiName, Utils.DataType.String);

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
                objdb.Write_log_file("GetServiceTypeClassName", x.Message);
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

        public static DataSet GetServiceTypeDetails(string xiFilter, string xiBUId)
        {
            string query = "select ct.id,ct.[name] as 'typename',ct.bu_id from bu_services_type ct where ct.active = 1 and ct.bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by ct.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataTable GetServiceType(string xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_services_type where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string DeleteServiceType(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from bu_services where type_id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteServiceType", x.Message);
            }
            finally
            {
                if (reader1 != null)
                {
                    reader1.Close();
                    reader1 = null;
                }
            }
            if (totalCount <= 0)
            {
                query = string.Format("update bu_services_type set active = 0 where id= {0}", xiId);
                int value = objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();
                returnValue = (value > 0) ? "Record deleted." : "false";
                return returnValue;
            }
            else
            {
                returnValue = "You can not delete this type because its already used.";
            }
            return returnValue;
        }

        #endregion
    }
}
