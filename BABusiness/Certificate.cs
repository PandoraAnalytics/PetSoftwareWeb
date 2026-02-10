using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace BABusiness
{
    public class Certificate : BusinessBase
    {
        public enum ApproveStatus
        {
            APPROVED = 1,
            REJECTED = -1,
        }

        #region Certificate

        public int AddCertificate(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [animal_certificate]([certificate_name],[startdate],[enddate],[submitdate],[certificate_file],[active],[animalid],[type],[status],[bu_id]) values(@certificate_name,@startdate,@enddate,getutcdate(),@files,1,@animalid,@type,@status,@companyid)";
            Parameter param1 = new Parameter("certificate_name", xiCollection["certificate_name"]);
            Parameter param2 = new Parameter("startdate", xiCollection["startdate"], DbType.Date);
            Parameter param3 = new Parameter("enddate", xiCollection["enddate"], DbType.Date);
            Parameter param4 = new Parameter("files", xiCollection["files"]);
            Parameter param5 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
            Parameter param6 = new Parameter("type", xiCollection["type"], DbType.Int32);
            Parameter param7 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param8 = new Parameter("companyid", xiCollection["companyid"], DbType.Int32);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query2 = "select scope_identity()";
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.ADDCERTIFICATE.ToString();
            logCollection["category"] = Common.AnimalLogCategory.CERTIFICATES.ToString();
            logCollection["description"] = xiCollection["certificate_name"];
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            if (value > 0)
                return value;
            else
                return int.MinValue;

        }

        public bool UpdateCertificate(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update animal_certificate set files=@files,certificate_name=@certificate_name,startdate=@startdate,enddate=@enddate,submitdate=getutcdate(),type=@type,status=@status where active=1 and id=@id";
            Parameter param1 = new Parameter("files", xiCollection["files"]);
            Parameter param2 = new Parameter("certificate_name", xiCollection["certificate_name"]);
            Parameter param3 = new Parameter("startdate", xiCollection["startdate"], DbType.Date);
            Parameter param4 = new Parameter("enddate", xiCollection["enddate"], DbType.Date);
            Parameter param5 = new Parameter("type", xiCollection["type"], DbType.Int32);
            Parameter param7 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param6 = new Parameter("id", xiId);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7 });
            objdb.Disconnectdb();
            return (value > 0);
        }

        public static int GetCertificateCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from animal_certificate c inner join animal_certificate_type ct on ct.id = c.[type] where c.active = 1";
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
                objdb.Write_log_file("GetCertificateCount", x.Message);
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

        public static DataSet GetCertificateDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;
            string query = "select c.*, ct.[type] as certificate_typename from animal_certificate c inner join animal_certificate_type ct on ct.id = c.[type]  where c.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.certificate_name offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("start_date", typeof(string));
                ds.Tables[0].Columns.Add("end_date", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate2 = Convert.ToDateTime(row["startdate"]);
                        if (tempDate2 != DateTime.MinValue) row["start_date"] = tempDate2.ToString(BusinessBase.DateFormat);
                    }
                    catch { }

                    if (row["enddate"] != DBNull.Value)
                    {
                        try
                        {
                            DateTime tempDate3 = Convert.ToDateTime(row["enddate"]);
                            if (tempDate3 != DateTime.MinValue) row["end_date"] = tempDate3.ToString(BusinessBase.DateFormat);
                        }
                        catch { }
                    }
                }

            }
            objdb.Disconnectdb();
            return ds;
        }

        public static bool DeleteCertificates(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update animal_certificate set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetCertificateDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select c.*, ct.[type] as certificate_typename from animal_certificate c inner join animal_certificate_type ct on ct.id = c.[type] where c.active=1 and c.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("certificate_name", Convert.ToString(objdb.GetValue(reader1, "certificate_name")));
                    collection.Add("certificate_file", Convert.ToString(objdb.GetValue(reader1, "certificate_file")));
                    collection.Add("startdate", Convert.ToString(objdb.GetValue(reader1, "startdate")));
                    collection.Add("enddate", Convert.ToString(objdb.GetValue(reader1, "enddate")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                    collection.Add("comments", Convert.ToString(objdb.GetValue(reader1, "comments")));
                    collection.Add("certificate_typename", Convert.ToString(objdb.GetValue(reader1, "certificate_typename")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));

                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCertificateDetail", x.Message);
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

            if (xiCollection["animalid"] != null && xiCollection["animalid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(animalid={0})", Utils.ConvertToDBString(xiCollection["animalid"], Utils.DataType.Integer)));
            }

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("([certificate_name] like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            //todo new chng
            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.bu_id is null or c.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public bool UpdateCertificatePic(string xiFileName, object xiId)
        {
            string query = "update animal_certificate set certificate_file=@certificatepic where id = @id and active=1";

            Parameter param1 = new Parameter("certificatepic", xiFileName);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        #endregion

        #region Certificate Type

        public int AddCertificatetype(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"insert into animal_certificate_type(type,ismandatory,active,createddate,breedtype,approval,[bu_id]) values(@type,@ismandatory,@active,getutcdate(),@breedtype,@approval,@companyid)";

            Parameter param1 = new Parameter("type", xiCollection["type"]);
            Parameter param2 = new Parameter("ismandatory", xiCollection["ismandatory"], DbType.Int16);
            Parameter param3 = new Parameter("active", 1);
            Parameter param4 = new Parameter("breedtype", xiCollection["breedtype"], DbType.String);
            Parameter param5 = new Parameter("approval", xiCollection["approval"], DbType.Int16);
            Parameter param6 = new Parameter("companyid", xiCollection["companyid"], DbType.Int32);
            string query2 = "select scope_identity()";

            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6 });
            objdb.Disconnectdb();

            if (value > 0)
                return value;
            else
                return int.MinValue;

        }

        public bool UpdateCertificatetype(NameValueCollection xiCollection, object xicertificatetypeId)
        {
            if (xiCollection == null) return false;

            string query = @"update animal_certificate_type set type=@type,ismandatory=@ismandatory,breedtype=@breedtype,createddate = getutcdate(),approval=@approval where id=@id";

            Parameter param1 = new Parameter("id", xicertificatetypeId, DbType.Int32);
            Parameter param2 = new Parameter("type", xiCollection["type"]);
            Parameter param3 = new Parameter("ismandatory", xiCollection["ismandatory"], DbType.Int16);
            Parameter param4 = new Parameter("breedtype", xiCollection["breedtype"], DbType.String);
            Parameter param5 = new Parameter("approval", xiCollection["approval"], DbType.Int16);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2, param3, param4, param1, param5 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string SearchType(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;
            if (!string.IsNullOrEmpty(xiCollection["mandatory"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("q.ismandatory = {0}", Utils.ConvertToDBString(xiCollection["mandatory"], Utils.DataType.Integer)));
            }
            if (!string.IsNullOrEmpty(xiCollection["association_breedtype"]))
            {
                string[] searchList = xiCollection["association_breedtype"].Split(',');
                if (searchList != null && searchList.Length > 0)
                {
                    string orQuery = "(q.breedtype is null)";
                    foreach (string search in searchList)
                    {
                        if (search.Length == 0) continue;
                        orQuery += string.Format(" or (','+q.breedtype+',' like {0})", Utils.ConvertToDBString("%," + search + ",%", Utils.DataType.String));
                    }
                    orQuery = "(" + orQuery + ")";

                    if (iswhere) builder.Append(" and ");
                    iswhere = true;
                    builder.Append(orQuery);
                }
            }
        
            // search txtbox 
            if (xiCollection["searchtext"] != null && xiCollection["searchtext"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(q.[type] like {0})", Utils.ConvertToDBString("%" + xiCollection["searchtext"] + "%", Utils.DataType.String)));
            }

            //new for bu_id
            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(q.bu_id is null or q.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }
            return builder.ToString();
        }

        public static DataSet GetAllCertificatetype(string xiFilter)
        {
            string query = "select q.id, q.ismandatory, q.[type],q.breedtype,q.active,q.bu_id from animal_certificate_type q where q.active = 1 ";
            if (!string.IsNullOrEmpty(xiFilter)) query += " and " + xiFilter;
            query += " order by q.type";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }
        public static string DeleteCertificatetype(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select Count(*) as maxcount from animal_certificate where  [type] =" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteCertificatetype", x.Message);
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
                query = string.Format("update animal_certificate_type set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));
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

        public static NameValueCollection GetCertificatetype(object xicertificatetypeId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from animal_certificate_type where id = " + Utils.ConvertToDBString(xicertificatetypeId, Utils.DataType.Integer) + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("type", Convert.ToString(objdb.GetValue(reader1, "type")));
                    collection.Add("ismandatory", Convert.ToString(objdb.GetValue(reader1, "ismandatory")));
                    collection.Add("approval", Convert.ToString(objdb.GetValue(reader1, "approval")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCertificatetype", x.Message);
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

        public static DataTable GetCertificateTypes(object xiBreedType)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from animal_certificate_type where active=1  and (breedtype is null or ','+breedtype+',' like " + Utils.ConvertToDBString("%," + xiBreedType + ",%", Utils.DataType.String) + ")  order by type";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetCertificateTypesByBuId(object xiBreedType, int xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = @"select id,[type] from animal_certificate_type where active=1 and (breedtype is null or ','+breedtype+',' like " + Utils.ConvertToDBString("%," + xiBreedType + ",%", Utils.DataType.String) + ")  " +
                "and bu_id is null or bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by type";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static NameValueCollection GetApprove(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select approval from animal_certificate_type where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("approval", Convert.ToString(objdb.GetValue(reader1, "approval")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetApprove", x.Message);
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

        #region Certificate Approval

        public bool UpdateStatus(NameValueCollection xiCollection, object xiCertId)
        {
            if (xiCollection == null) return false;

            string query = "update animal_certificate set [status]=@status,[comments]=@comments where id=@id";

            Parameter param1 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param2 = new Parameter("comments", xiCollection["comments"]);
            Parameter param3 = new Parameter("id", xiCertId, DbType.Int32);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string SearchApprove(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;
            if (xiCollection["title"] != null && xiCollection["title"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.[certificate_name] like {0})", Utils.ConvertToDBString("%" + xiCollection["title"] + "%", Utils.DataType.String)));
            }
            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(u.fname + ' ' + u.lname like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }
            if (xiCollection["animal"] != null && xiCollection["animal"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.[name] like {0})", Utils.ConvertToDBString("%" + xiCollection["animal"] + "%", Utils.DataType.String)));
            }
            if (!string.IsNullOrEmpty(xiCollection["association_breedtype"]))
            {
                string[] searchList = xiCollection["association_breedtype"].Split(',');
                if (searchList != null && searchList.Length > 0)
                {
                    string orQuery = "(ct.breedtype is null)";
                    foreach (string search in searchList)
                    {
                        if (search.Length == 0) continue;
                        orQuery += string.Format(" or (','+ct.breedtype+',' like {0})", Utils.ConvertToDBString("%," + search + ",%", Utils.DataType.String));
                    }
                    orQuery = "(" + orQuery + ")";

                    if (iswhere) builder.Append(" and ");
                    iswhere = true;
                    builder.Append(orQuery);
                }
            }

            return builder.ToString();
        }

        public static DataSet GetAllCertificatesToApprove(int xiPage, string xiFilter)
        {
            xiPage = (xiPage == 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select c.id, c.certificate_name, ct.[type] as certificate_typename, c.submitdate, c.startdate, c.enddate, c.certificate_file, a.name, a.typename, u.lname as lname, u.fname as fname
from animal_certificate c 
inner join animal_certificate_type ct on ct.id = c.[type] 
inner join view_animal a on c.animalid = a.id
inner join user_animal ua on ua.animalid  = a.id
inner join [user] u on u.id = ua.userid and u.active =1
where c.active = 1 and c.[status] =0";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.id desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                ds.Tables[0].Columns.Add("procesed_startdate", typeof(string));
                ds.Tables[0].Columns.Add("procesed_enddate", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate1 = Convert.ToDateTime(row["startdate"]);
                        if (tempDate1 != DateTime.MinValue) row["procesed_startdate"] = tempDate1.ToString(BusinessBase.DateFormat);
                    }
                    catch { }

                    try
                    {
                        if (row["enddate"] != DBNull.Value)
                        {
                            DateTime tempDate2 = Convert.ToDateTime(row["enddate"]);
                            if (tempDate2 != DateTime.MinValue) row["procesed_enddate"] = tempDate2.ToString(BusinessBase.DateFormat);
                        }
                    }
                    catch { }
                }
            }

            return ds;
        }

        public static int GetAllCertificatesToApproveCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(ua.id) as maxcount from animal_certificate c 
inner join animal_certificate_type ct on ct.id = c.[type] 
inner join view_animal a on c.animalid = a.id
inner join user_animal ua on ua.animalid  = a.id
inner join [user] u on u.id = ua.userid and u.active =1
where c.active = 1 and c.[status] =0 ";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAllCertificatesToApproveCount", x.Message);
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

        public static DataTable GetNotProvidedMandatoryCertificates(object xiAnimalId)
        {
            string query = @"select ct.id, ct.[type] from animal_certificate_type ct where ct.active = 1 and ct.ismandatory = 1
and (ct.breedtype is null or ','+ct.breedtype+',' like '%,'+(select cast(breedtype as nvarchar) from animal where id = {0})+',%')
and ct.id not in 
(select distinct isnull(c.[type], 0) from animal_certificate c where c.active = 1 and c.status in (0,1) and c.animalid = {0})";

            query = string.Format(query, Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer));
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dataTable;
        }

        #endregion
    }
}
