using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BABusiness
{
    public class CustomFields
    {
        public enum enum_customfieldsType
        {
            OneLine = 1,
            Paragraph = 2,
            List = 3,
            Singleselect = 4,
            Multiselect = 5,
            Fileupload = 6,
            Range = 7,
            Matrix = 8,
            Date = 9,
            Time = 10
        }

        public int Add(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            if (!xiCollection.AllKeys.Contains("moreinfo")) xiCollection["moreinfo"] = string.Empty;
            if (!xiCollection.AllKeys.Contains("moreinfo_link")) xiCollection["moreinfo_link"] = string.Empty;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"insert into animal_customfields(type,title,ismandatory,minval,maxval,fileextension,active,createdby,createddate,breedtype,sortorder,rowvalue,bu_id)
values(@type, @title, @ismandatory, @minval, @maxval, @fileextension,@active,@createdby, getutcdate(),@breedtype,@sortorder,@rowvalue,@companyid)";

            Parameter param2 = new Parameter("type", xiCollection["type"]);
            Parameter param3 = new Parameter("title", xiCollection["title"]);
            Parameter param4 = new Parameter("ismandatory", xiCollection["ismandatory"], DbType.Int16);
            Parameter param9 = new Parameter("minval", xiCollection["minval"], DbType.Int32);
            Parameter param10 = new Parameter("maxval", xiCollection["maxval"], DbType.Int32);
            Parameter param11 = new Parameter("fileextension", xiCollection["fileextension"]);
            Parameter param12 = new Parameter("active", 1);
            Parameter param13 = new Parameter("createdby", xiCollection["createdby"], DbType.Int32);
            Parameter param14 = new Parameter("breedtype", xiCollection["breedtype"], DbType.String);
            Parameter param17 = new Parameter("sortorder", xiCollection["sortorder"], DbType.String);
            Parameter param18 = new Parameter("rowvalue", xiCollection["rowvalue"], DbType.Int32);
            Parameter param19 = new Parameter("companyid", xiCollection["companyid"], DbType.Int32);

            string query2 = "select scope_identity()";

            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param2, param3, param4, param9, param10, param11, param12, param13, param14, param17, param18, param19 });
            objdb.Disconnectdb();

            if (value > 0)
                return value;
            else
                return int.MinValue;

        }

        public bool Update(NameValueCollection xiCollection, object xicustomfieldsId)
        {
            if (xiCollection == null) return false;

            if (!xiCollection.AllKeys.Contains("moreinfo")) xiCollection["moreinfo"] = string.Empty;
            if (!xiCollection.AllKeys.Contains("moreinfo_link")) xiCollection["moreinfo_link"] = string.Empty;

            string query = @"update animal_customfields set title=@title,minval=@minval,maxval=@maxval,fileextension=@fileextension,ismandatory=@ismandatory,
breedtype=@breedtype,updateddate = getutcdate(),rowvalue = @rowvalue where id=@id and active=1";

            Parameter param2 = new Parameter("id", xicustomfieldsId, DbType.Int32);
            Parameter param3 = new Parameter("title", xiCollection["title"]);
            Parameter param4 = new Parameter("ismandatory", xiCollection["ismandatory"], DbType.Int16);
            Parameter param9 = new Parameter("minval", xiCollection["minval"], DbType.Int32);
            Parameter param10 = new Parameter("maxval", xiCollection["maxval"], DbType.Int32);
            Parameter param11 = new Parameter("fileextension", xiCollection["fileextension"]);
            Parameter param14 = new Parameter("breedtype", xiCollection["breedtype"], DbType.String);
            Parameter param15 = new Parameter("rowvalue", xiCollection["rowvalue"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2, param3, param4, param9, param10, param11, param14, param15 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string Search(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;
            if (!string.IsNullOrEmpty(xiCollection["title"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("q.title like {0}", Utils.ConvertToDBString("%" + xiCollection["title"] + "%", Utils.DataType.String)));
            }
            if (!string.IsNullOrEmpty(xiCollection["type"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("q.[breedtype] = {0}", Utils.ConvertToDBString(xiCollection["type"], Utils.DataType.String)));
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
            if (!string.IsNullOrEmpty(xiCollection["iseventtype"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append("q.[breedtype] = '0'");
            }

            if (!string.IsNullOrEmpty(xiCollection["ischecklistype"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append("q.[breedtype] = 'checklist'");
            }

            if (!string.IsNullOrEmpty(xiCollection["companyid"]))
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(q.[bu_id] is null or q.[bu_id] = {0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static NameValueCollection GetCustomFields(object xicustomfieldsId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from animal_customfields where id = " + Utils.ConvertToDBString(xicustomfieldsId, Utils.DataType.Integer) + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();

                    collection.Add("type", Convert.ToString(objdb.GetValue(reader1, "type")));
                    collection.Add("title", Convert.ToString(objdb.GetValue(reader1, "title")));
                    collection.Add("ismandatory", Convert.ToString(objdb.GetValue(reader1, "ismandatory")));
                    collection.Add("minval", Convert.ToString(objdb.GetValue(reader1, "minval")));
                    collection.Add("maxval", Convert.ToString(objdb.GetValue(reader1, "maxval")));
                    collection.Add("fileextension", Convert.ToString(objdb.GetValue(reader1, "fileextension")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "group_name")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("sortorder", Convert.ToString(objdb.GetValue(reader1, "sortorder")));
                    collection.Add("rowvalue", Convert.ToString(objdb.GetValue(reader1, "rowvalue")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("Getcustomfields", x.Message);
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

        public static DataTable GetCustomFieldsOptions(object xicustomfieldsId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select id, optiontext from animal_customfieldsoption where fieldid = " + Utils.ConvertToDBString(xicustomfieldsId, Utils.DataType.Integer) + "";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataSet GetAllCustomFields(int xiPage, string xiFilter, string xiActive)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;
            string activefilter = (xiActive == "2") ? "q.active=2" : "q.active=1";

            string query = "select q.id, q.title, [type],q.active from animal_customfields q where " + activefilter;
            if (!string.IsNullOrEmpty(xiFilter)) query += " and " + xiFilter;
            query += " order by q.sortorder,q.title offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static int GetAllCustomFieldsCount(string xiFilter, string xiActive)
        {
            string activefilter = (xiActive == "2") ? "q.active=2" : "q.active=1";
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(q.id) as maxcount from animal_customfields q where " + activefilter;
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAllCustomFieldsCount", x.Message);
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

        public static DataSet GetValueByCustomFields(string xicustomfieldsId)
        {
            string query = "select * from animal_customfields_values where fieldid = " + Utils.ConvertToDBString(xicustomfieldsId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }
        public static string DeleteCustomFields(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = @"select Count(*) as maxcount from animal_customfields_values a inner
            join animal_customfields ac on a.fieldid = ac.id where  a.[fieldid] = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and ac.active = 1";
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
                objdb.Write_log_file("DeleteCustomFields", x.Message);
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
                query = string.Format("update animal_customfields set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));
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


        public static bool ShowHideCustomField(object xiCustomId, bool xiShow)
        {
            string query = string.Empty;
            if (xiShow)
            {
                // make it active again
                query = @"update [animal_customfields] set active=1 from [animal_customfields] where id in ({0});";
            }
            else
            {
                // set active to 2 not 0, 0 is delete and 2 is hidden and delete_user_id = 999
                query = "update animal_customfields set active=2 where id in ({0});";
            }
            query = string.Format(query, xiCustomId);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        #region Options

        public bool AddOptions(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into [animal_customfieldsoption](fieldid,optiontext,optionkey) values(@fieldid,@optiontext,@optionkey)";

            Parameter param1 = new Parameter("fieldid", xiCollection["fieldid"], DbType.Int32);
            Parameter param2 = new Parameter("optiontext", xiCollection["optiontext"]);
            Parameter param3 = (string.IsNullOrEmpty(xiCollection["optionkey"])) ? new Parameter("optionkey", DBNull.Value) : new Parameter("optionkey", xiCollection["optionkey"], DbType.String);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateOptions(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "update animal_customfieldsoption set optiontext = @optiontext where id = @id";

            Parameter param1 = new Parameter("id", xiCollection["id"], DbType.Int32);
            Parameter param2 = new Parameter("optiontext", xiCollection["optiontext"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool DeleteOption(string xiOptionId)
        {
            string query = "delete animal_customfieldsoption where id =  @id";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            Parameter param1 = new Parameter("id", xiOptionId, DbType.Int32);
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool DeleteQuestionOptions(int xiFieldId)
        {
            string query = "delete animal_customfieldsoption where fieldid = " + Utils.ConvertToDBString(xiFieldId, Utils.DataType.Integer) + "";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        #endregion


    }
}
