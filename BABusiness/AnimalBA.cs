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
    public class AnimalBA : BusinessBase
    {
        #region Animal

        public int AddBreed(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [animal]([name],[animalcategory],[breedtype],[dob],[collar_id],[aboutme],[profilepic_file],[fathername],[mothername],[active],[gender],[submitdate]) values(";
            query += Utils.ConvertToDBString(xiCollection["name"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["animalcategory"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["type"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["date"], Utils.DataType.Date) + ", ";
            query += Utils.ConvertToDBString(xiCollection["collarid"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["about"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString("", Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString("", Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString("", Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString("1", Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["gender"], Utils.DataType.Integer) + ",";
            query += "getutcdate()); ";

            // }

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int animalid = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2);
            if (animalid > 0)
            {
                query = "insert into [user_animal]([userid],[animalid],[active],[submitdate]) values(";
                query += Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer) + ", ";
                query += Utils.ConvertToDBString(animalid, Utils.DataType.Integer) + ", ";
                query += "1, getutcdate()); ";

                objdb.ExecuteNonQuery(objdb.con, query);
            }
            objdb.Disconnectdb();

            return animalid;
        }

        public bool UpdateAnimal(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = @"update [animal] set name=@name,breedtype=@breedtype,dob=@dob,collar_id=@collar_id,aboutme=@aboutme,gender=@gender,height=@height,weight=@weight,life=@life,spancoat=@spancoat,dog_breed_group=@dog_breed_group,color=@color,classtypeid=@classtypeid where id=@id and active=1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param3 = new Parameter("breedtype", xiCollection["type"], DbType.Int32);
            Parameter param4 = new Parameter("dob", xiCollection["date"], DbType.Date);
            Parameter param5 = new Parameter("collar_id", xiCollection["collarid"], DbType.String);
            Parameter param6 = new Parameter("aboutme", xiCollection["about"]);
            Parameter param7 = new Parameter("gender", xiCollection["gender"], DbType.Int32);
            Parameter param8 = new Parameter("height", xiCollection["height"]);
            Parameter param9 = new Parameter("weight", xiCollection["weight"]);
            Parameter param10 = new Parameter("life", xiCollection["life"]);
            Parameter param11 = new Parameter("spancoat", xiCollection["spancoat"]);
            Parameter param12 = new Parameter("dog_breed_group", xiCollection["dog_breed_group"]);
            Parameter param13 = new Parameter("color", xiCollection["color"]);
            Parameter param14 = new Parameter("classtypeid", xiCollection["classtypeid"], DbType.Int32);
            Parameter param15 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = BusinessBase.ConvertToString(xiId);
            logCollection["key"] = Common.AnimalLogKey.EDITBASICINFO.ToString();
            logCollection["category"] = Common.AnimalLogCategory.BASICINFO.ToString();
            logCollection["description"] = null;
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public bool UpdateAnimalProfilePic(string xiFileName, object xiId)
        {
            string query = "update [animal] set profilepic_file=@profilepic where id=@id and active=1";

            Parameter param7 = new Parameter("profilepic", xiFileName);
            Parameter param11 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param7, param11 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateAnimalParent(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = @"update [animal] set fathername=@fathername, fatherid=(select top 1 a.id from animal a inner join user_animal ua on a.id=ua.animalid where a.[name]=@fathername and a.active=1 and ua.userid=@userid),
mothername=@mothername,motherid=(select top 1 a.id from animal a inner join user_animal ua on a.id=ua.animalid where a.[name]=@mothername and a.active=1 and ua.userid=@userid)
where id=@id and active=1";
            Parameter param1 = new Parameter("fathername", xiCollection["fathername"]);
            Parameter param2 = new Parameter("mothername", xiCollection["mothername"]);
            Parameter param3 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param4 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = BusinessBase.ConvertToString(xiId);
            logCollection["key"] = Common.AnimalLogKey.EDITPARENT.ToString();
            logCollection["category"] = Common.AnimalLogCategory.PARENTS.ToString();
            logCollection["description"] = null;
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public static NameValueCollection GetAnimalDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from view_animal  where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("animalcategory", Convert.ToString(objdb.GetValue(reader1, "animalcategory")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("dob", Convert.ToString(objdb.GetValue(reader1, "dob")));
                    collection.Add("collar_id", Convert.ToString(objdb.GetValue(reader1, "collar_id")));
                    collection.Add("aboutme", Convert.ToString(objdb.GetValue(reader1, "aboutme")));
                    collection.Add("profilepic_file", Convert.ToString(objdb.GetValue(reader1, "profilepic_file")));
                    collection.Add("fathername", Convert.ToString(objdb.GetValue(reader1, "fathername")));
                    collection.Add("mothername", Convert.ToString(objdb.GetValue(reader1, "mothername")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("categoryname", Convert.ToString(objdb.GetValue(reader1, "categoryname")));
                    collection.Add("typename", Convert.ToString(objdb.GetValue(reader1, "typename")));
                    collection.Add("breedimage", Convert.ToString(objdb.GetValue(reader1, "breedimage")));
                    collection.Add("fatherid", Convert.ToString(objdb.GetValue(reader1, "fatherid")));
                    collection.Add("motherid", Convert.ToString(objdb.GetValue(reader1, "motherid")));
                    collection.Add("gender", Convert.ToString(objdb.GetValue(reader1, "gender")));
                    collection.Add("height", Convert.ToString(objdb.GetValue(reader1, "height")));
                    collection.Add("weight", Convert.ToString(objdb.GetValue(reader1, "weight")));
                    collection.Add("life", Convert.ToString(objdb.GetValue(reader1, "life")));
                    collection.Add("spancoat", Convert.ToString(objdb.GetValue(reader1, "spancoat")));
                    collection.Add("dog_breed_group", Convert.ToString(objdb.GetValue(reader1, "dog_breed_group")));
                    collection.Add("color", Convert.ToString(objdb.GetValue(reader1, "color")));

                    collection.Add("classtypeid", Convert.ToString(objdb.GetValue(reader1, "classtypeid")));
                    collection.Add("breederemail", Convert.ToString(objdb.GetValue(reader1, "breederemailid")));
                    collection.Add("breedername", Convert.ToString(objdb.GetValue(reader1, "breederfullname")));
                    collection.Add("breederid", Convert.ToString(objdb.GetValue(reader1, "breederid")));
                    collection.Add("classtypename", Convert.ToString(objdb.GetValue(reader1, "classtypename")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalDetail", x.Message);
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

        public static NameValueCollection GetAnimalDetailByName(object xiName)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from view_animal where active=1 and [name]=" + Utils.ConvertToDBString(xiName, Utils.DataType.String);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("animalcategory", Convert.ToString(objdb.GetValue(reader1, "animalcategory")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("dob", Convert.ToString(objdb.GetValue(reader1, "dob")));
                    collection.Add("collar_id", Convert.ToString(objdb.GetValue(reader1, "collar_id")));
                    collection.Add("aboutme", Convert.ToString(objdb.GetValue(reader1, "aboutme")));
                    collection.Add("profilepic_file", Convert.ToString(objdb.GetValue(reader1, "profilepic_file")));
                    collection.Add("fathername", Convert.ToString(objdb.GetValue(reader1, "fathername")));
                    collection.Add("mothername", Convert.ToString(objdb.GetValue(reader1, "mothername")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("categoryname", Convert.ToString(objdb.GetValue(reader1, "categoryname")));
                    collection.Add("typename", Convert.ToString(objdb.GetValue(reader1, "typename")));
                    collection.Add("typeclassname", Convert.ToString(objdb.GetValue(reader1, "typeclassname")));
                    collection.Add("classtypeid", Convert.ToString(objdb.GetValue(reader1, "classtypeid")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalDetail", x.Message);
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

        public bool DeleteAnimal(object xiId, string xiUserId, string xiReason)
        {
            DBClass objdb = new DBClass();
            string query = string.Format("delete from  user_animal where userid=@userid and animalid=@id;update animal set fatherid=null where fatherid=@id;update animal set motherid=null where motherid=@id;update animal set active = 0,reasonofdelete=@reason where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            Parameter param2 = new Parameter("userid", xiUserId, DbType.Int32);
            Parameter param3 = new Parameter("reason", xiReason);

            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool DeactivateAnimal(object xiId, string xiReason)
        {
            DBClass objdb = new DBClass();
            string query = string.Format("update animal set active = 2,reasonofdelete=@reason where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            Parameter param2 = new Parameter("reason", xiReason);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool ActivateAnimal(object xiId)
        {
            DBClass objdb = new DBClass();
            string query = string.Format("update animal set active = 1,reasonofdelete = null where id= @id and active=2");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetAnimalCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(id) as maxcount from [animal] where active = 1";
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
                objdb.Write_log_file("GetAnimalCount", x.Message);
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

        public static DataSet GetAnimalDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select * from [animal] where active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by id offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("lastmodified", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["lastmodified"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                    }
                }
            }
            objdb.Disconnectdb();
            return ds;
        }

        public static DataTable GetAllAnimalsByCategory(string xiCategory, string xiUserId)
        {
            string query = "select [name] from [animal] a inner join user_animal ua on ua.animalid = a.id and ua.userid = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " where a.active = 1 and a.animalcategory = " + Utils.ConvertToDBString(xiCategory, Utils.DataType.Integer) + "  order by [name]";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetAnimalDetailsReadonly(string xiId)
        {
            string query = "select * from [view_animal] active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string AnimalSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["animalid"] != null && xiCollection["animalid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(animalid={0})", Utils.ConvertToDBString(xiCollection["animalid"], Utils.DataType.Integer)));
            }
            return builder.ToString();
        }

        public static DataTable GetAnimalChilds(object xiAnimalId)
        {
            string query = "animal_getchilds";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteStoreProcDataTable(objdb.con, query, new Parameter[] { new Parameter("animalid", xiAnimalId, DbType.Int32) });
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetAnimalParents(object xiAnimalId)
        {
            string query = "animal_getparents";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteStoreProcDataTable(objdb.con, query, new Parameter[] { new Parameter("animalid", xiAnimalId, DbType.Int32) });
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetAllAnimalBreedGroupCategory(string xiCategory)
        {
            string query = "select distinct dog_breed_group from animal where dog_breed_group is not null and animalcategory = " + Utils.ConvertToDBString(xiCategory, Utils.DataType.Integer) + " order by dog_breed_group";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public bool UpdateAnimalBreederInfo(NameValueCollection xiCollection, object xiId)
        {
            string query = "update [animal] set breederid=@breederid,breedername=@breedername,breederemail=@breederemail where id=@id and active=1";

            Parameter param1 = string.IsNullOrEmpty(xiCollection["breederid"]) ? new Parameter("breederid", DBNull.Value) : new Parameter("breederid", xiCollection["breederid"], DbType.Int32);
            Parameter param2 = string.IsNullOrEmpty(xiCollection["breedername"]) ? new Parameter("breedername", DBNull.Value) : new Parameter("breedername", xiCollection["breedername"]);
            Parameter param3 = string.IsNullOrEmpty(xiCollection["breederemail"]) ? new Parameter("breederemail", DBNull.Value) : new Parameter("breederemail", xiCollection["breederemail"]);

            Parameter param4 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetEventsAnimal(string xiEventId)
        {
            string query = "select * from view_animal where id in (select [value] from event_registrations c CROSS APPLY STRING_SPLIT_2(c.animallist,',') left join animal a on a.id= value where eventid = " + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer) + ")";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetEventsOwners(string xiEventId)
        {
            string query = "select r.id, u.fname + ' ' + u.lname as 'ownername', u.[address], u.city, u.pincode, u.countryid, c.fullname, IIF(u.[address] IS NULL, ' ',u.[address]+',') + IIF(u.city IS NULL, ' ', u.city+',')+IIF(u.pincode IS NULL, ' ', CAST(u.pincode as nvarchar(10))+',')+IIF(c.fullname IS NULL, ' ', c.fullname) as 'fulladdress' from event_registrations r inner join [user] u on r.userid = u.id and u.active =1 inner join countries c on c.id = u.countryid where eventid = " + Utils.ConvertToDBString(xiEventId, Utils.DataType.Integer) + "";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        #endregion

        #region Animal_Gallery

        public static bool AddAnimalGallery(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [animal_gallery]([animalid],[title],[gallery_file],[file_type],[active],[submitdate],[bu_id]) values(@animalid,@title,@file_name,@file_type,1,getutcdate(),@companyid)";
            Parameter param1 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
            Parameter param2 = new Parameter("file_name", xiCollection["file_name"]);
            Parameter param3 = new Parameter("title", xiCollection["title"]);
            Parameter param4 = new Parameter("file_type", xiCollection["file_type"], DbType.Int32);
            Parameter param5 = new Parameter("companyid", xiCollection["companyid"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.ADDGALLERY.ToString();
            logCollection["category"] = Common.AnimalLogCategory.GALLERY.ToString();
            logCollection["description"] = null;
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public static DataSet GetAnimalGalleryPhotos(object xiAnimalId)
        {
            string query = "select * from animal_gallery where active=1 and animalid=" + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer) + " order by submitdate desc";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static DataSet GetCompanyAnimalGalleryPhotos(object xiAnimalId, object xiCompanyId)
        {
            string query = "select * from animal_gallery where active=1 and animalid= {0} and (bu_id is null or bu_id={1}) order by submitdate desc";

            query = string.Format(query, Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer), Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static bool DeleteAnimalGalleryPhoto(string xiFileName, NameValueCollection xiCollection)
        {
            string query = "delete from [animal_gallery] where [gallery_file]=@file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();
            if (xiCollection != null)
            {
                NameValueCollection logCollection = new NameValueCollection();
                logCollection["animalid"] = xiCollection["animalid"];
                logCollection["key"] = Common.AnimalLogKey.DELETEGALLERY.ToString();
                logCollection["category"] = Common.AnimalLogCategory.GALLERY.ToString();
                logCollection["description"] = xiFileName;
                logCollection["userid"] = xiCollection["userid"];
                Common.SaveAnimalLog(logCollection);
            }
            return (value > 0);

        }

        #endregion

        #region Animal_Food

        public bool AddAnimalFood(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;
            string query = "insert into [animal_food]([food],[day],[day_interval],[quantity],[unit],[active],[submitdate],[animalid],[foodtype],[bu_id]) values(@food,@day,@day_interval,@quantity,@unit,@active,getutcdate(),@animalid,@foodtype,@companyid)";
            Parameter param1 = new Parameter("food", xiCollection["food"]);
            Parameter param2 = new Parameter("day", xiCollection["day"], DbType.String);
            Parameter param3 = new Parameter("day_interval", xiCollection["day_interval"], DbType.String);
            Parameter param4 = new Parameter("quantity", xiCollection["quantity"], DbType.Int32);
            Parameter param5 = new Parameter("unit", xiCollection["unit"]);
            Parameter param6 = new Parameter("active", "1");
            Parameter param7 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
            Parameter param8 = new Parameter("foodtype", xiCollection["foodtype"], DbType.Int32);
            Parameter param9 = new Parameter("companyid", xiCollection["companyid"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.ADDFOOD.ToString();
            logCollection["category"] = Common.AnimalLogCategory.FOOD.ToString();
            logCollection["description"] = xiCollection["food"];
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);


            return (value > 0);

        }

        public bool UpdateAnimalFood(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update animal_food set food=@food,day=@day,day_interval=@day_interval,quantity=@quantity,unit=@unit,foodtype=@foodtype where active=1 and id=@id";

            Parameter param1 = new Parameter("food", xiCollection["food"]);
            Parameter param2 = new Parameter("day", xiCollection["day"], DbType.String);
            Parameter param3 = new Parameter("day_interval", xiCollection["day_interval"], DbType.String);
            Parameter param4 = new Parameter("quantity", xiCollection["quantity"], DbType.Int32);
            Parameter param5 = new Parameter("unit", xiCollection["unit"]);
            Parameter param6 = new Parameter("foodtype", xiCollection["foodtype"], DbType.Int32);
            Parameter param7 = new Parameter("id", xiId, DbType.Int32);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.EDITFOOD.ToString();
            logCollection["category"] = Common.AnimalLogCategory.FOOD.ToString();
            logCollection["description"] = xiCollection["food"];
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public static int GetAnimalFoodCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(id) as maxcount from animal_food where active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));

            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalFoodCount", x.Message);
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

        public static DataSet GetAnimalFoodDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;
            string query = "select * from animal_food where active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by submitdate desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

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
                        row["procesed_date"] = tempDate1.ToString(BusinessBase.DateFormat);
                    }
                }
            }

            return ds;
        }

        public static bool DeleteAnimalFood(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update animal_food set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetAnimalFoodDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from [animal_food] where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("food", Convert.ToString(objdb.GetValue(reader1, "food")));
                    collection.Add("day", Convert.ToString(objdb.GetValue(reader1, "day")));
                    collection.Add("day_interval", Convert.ToString(objdb.GetValue(reader1, "day_interval")));
                    collection.Add("quantity", Convert.ToString(objdb.GetValue(reader1, "quantity")));
                    collection.Add("unit", Convert.ToString(objdb.GetValue(reader1, "unit")));
                    collection.Add("file", Convert.ToString(objdb.GetValue(reader1, "file")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("animalid", Convert.ToString(objdb.GetValue(reader1, "animalid")));
                    collection.Add("foodtype", Convert.ToString(objdb.GetValue(reader1, "foodtype")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalFoodDetail", x.Message);
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

        public static string FoodSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["animalid"] != null && xiCollection["animalid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(animalid={0})", Utils.ConvertToDBString(xiCollection["animalid"], Utils.DataType.Integer)));
            }

            if (xiCollection["food"] != null && xiCollection["food"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("([food] like {0})", Utils.ConvertToDBString("%" + xiCollection["food"] + "%", Utils.DataType.String)));
            }
            //todo new chng
            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(bu_id is null or bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }
            return builder.ToString();
        }

        #endregion

        #region Animal_Notes

        public int AddAnimalNotes(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [animal_notes]([description],[submitdate],[active],[animalid],[bu_id]) values(@description,@date,1,@animalid,@companyid)";

            Parameter param1 = new Parameter("date", xiCollection["date"], DbType.Date);
            Parameter param2 = new Parameter("description", xiCollection["note"]);
            Parameter param5 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
            Parameter param4 = new Parameter("companyid", xiCollection["companyid"], DbType.Int32);
         
            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int noteId = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param5, param4 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.ADDNOTE.ToString();
            logCollection["category"] = Common.AnimalLogCategory.NOTES.ToString();
            logCollection["description"] = (xiCollection["note"].Length > 100) ? xiCollection["note"].Substring(0, 100) : xiCollection["note"];
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return noteId;
        }

        public bool UpdateAnimalNotes(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update animal_notes set submitdate=@date,description=@description where id=@id and active=1 ";

            Parameter param1 = new Parameter("date", xiCollection["date"], DbType.Date);
            Parameter param2 = new Parameter("description", xiCollection["note"]);
            Parameter param4 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param4 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.EDITNOTE.ToString();
            logCollection["category"] = Common.AnimalLogCategory.NOTES.ToString();
            logCollection["description"] = (xiCollection["note"].Length > 100) ? xiCollection["note"].Substring(0, 100) : xiCollection["note"];
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public static int GetAnimalNotesCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(n.id) as maxcount from animal_notes n where n.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));

            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalNotesCount", x.Message);
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

        public static DataSet GetAnimalNotesDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;
            string query = "select n.*, (select count(*) from animal_notes_files where noteid=n.id) as filescount from animal_notes n where n.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by n.submitdate desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

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
                        row["procesed_date"] = tempDate1.ToString(BusinessBase.DateFormat);
                    }
                }
            }
            return ds;
        }

        public static bool DeleteAnimalNotes(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update animal_notes set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetAnimalNotesDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from [animal_notes] where id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1"; ;

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("animalid", Convert.ToString(objdb.GetValue(reader1, "animalid")));
                    collection.Add("description", Convert.ToString(objdb.GetValue(reader1, "description")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalNotesDetail", x.Message);
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

        public static string NoteSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["animalid"] != null && xiCollection["animalid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(n.animalid={0})", Utils.ConvertToDBString(xiCollection["animalid"], Utils.DataType.Integer)));
            }

            if (xiCollection["description"] != null && xiCollection["description"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(n.description like {0})", Utils.ConvertToDBString("%" + xiCollection["description"] + "%", Utils.DataType.String)));
            }

            //todo new chng
            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(n.bu_id is null or n.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }

         
            return builder.ToString();
        }

        #endregion

        #region Animal_Notes_Files

        public static bool AddAnimalNotes_Files(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [animal_notes_files]([noteid],[file]) values(@noteid,@file)";
            Parameter param1 = new Parameter("noteid", xiCollection["noteid"], DbType.Int32);
            Parameter param2 = new Parameter("file", xiCollection["file"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);

        }

        public static DataTable GetAnimalNotes_FilesDetails(object xiNoteId)
        {
            string query = "select * from animal_notes_files where noteid=" + Utils.ConvertToDBString(xiNoteId, Utils.DataType.Integer);

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

        public static bool DeleteAnimalNotes_Files(string xiFileName)
        {
            string query = "delete from [animal_notes_files] where [file]=@file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();

            return (value > 0);

        }

        #endregion

        #region Animal_Appointment

        public int AddAnimalAppointment(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = @"insert into [animal_appointment]([animalid],[appointmentdate],[contactid],[remind_before_number],[remind_before_text],[active],[submitdate],[bu_id]) values(@animalid,@date,@contactid,@remind_before_no,@remind_before_text,1,getutcdate(),@companyid)";
            Parameter param1 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
            Parameter param2 = new Parameter("date", xiCollection["datetime"], DbType.DateTime);
            Parameter param3 = new Parameter("contactid", xiCollection["contact"], DbType.Int32);
            Parameter param4 = new Parameter("companyid", xiCollection["companyid"], DbType.Int32);
            Parameter param6 = new Parameter("remind_before_no", xiCollection["remindnumber"], DbType.Int32);
            Parameter param7 = new Parameter("remind_before_text", xiCollection["remindtext"]);           

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int appointmentid = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param6, param7 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.ADDAPPOINTMENT.ToString();
            logCollection["category"] = Common.AnimalLogCategory.APPOINTMENTS.ToString();
            logCollection["description"] = xiCollection["appdates"]; // Appointment is booked from 10/07/2023 - 30/07/2023 for ABCAnimal
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return appointmentid;

        }

        public bool AddAnimalAppointmentDates(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "insert into animal_appointment_dates(appointmentid,startdatetime,enddatetime,seqno,status) values(@appointmentid,@startdate,@enddate, (select count(*)+1 from animal_appointment_dates where appointmentid = @appointmentid),1)";

            Parameter param1 = new Parameter("appointmentid", xiCollection["appointmentid"], DbType.Int32);
            Parameter param2 = new Parameter("startdate", xiCollection["startdate"], DbType.DateTime);
            Parameter param3 = new Parameter("enddate", xiCollection["startdate"], DbType.DateTime); // startdate and enddate is same

            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool AddAnimalAppointmentFiles(string xiFileName, object xiAppointmentId)
        {
            string query = "insert into animal_appointment_files(appointmentid,[file]) values(@appointmentid,@file)";

            Parameter param1 = new Parameter("appointmentid", xiAppointmentId, DbType.Int32);
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateAnimalAppointment(NameValueCollection xiCollection, object xiId, bool xiThisOccuranceAndFollowing)
        {
            if (xiCollection == null) return false;

            string query = "";

            if (xiThisOccuranceAndFollowing)
            {
                //query = "update animal_appointment_dates set startdatetime = dateadd(minute, (SELECT DATEDIFF(minute, startdatetime, @date)), startdatetime), enddatetime=startdatetime where id >= @dateid and appointmentid = @appointmentid and active=1;";
                query = "update animal_appointment_dates set startdatetime = dateadd(minute, (SELECT DATEDIFF(minute, cast(startdatetime as time), cast(@date as time))), startdatetime), enddatetime = startdatetime where id >= @dateid and appointmentid = @appointmentid and active = 1";
            }
            else
            {
                //query = "update animal_appointment_dates set startdatetime = dateadd(minute, (SELECT DATEDIFF(minute, startdatetime, @date)), startdatetime), enddatetime=startdatetime where id = @dateid and appointmentid = @appointmentid and active=1;";
                query = "update animal_appointment_dates set startdatetime = dateadd(minute, (SELECT DATEDIFF(minute, cast(startdatetime as time), cast(@date as time))), startdatetime), enddatetime = startdatetime where id = @dateid and appointmentid = @appointmentid and active = 1";
            }

            query += "update animal_appointment set contactid=@contactid,remind_before_number=@remind_before_number,remind_before_text=@remind_before_text where active=1 and id=@appointmentid";

            Parameter param1 = new Parameter("contactid", xiCollection["contactid"], DbType.Int32);
            Parameter param2 = new Parameter("date", xiCollection["datetime"], DbType.DateTime);
            Parameter param3 = new Parameter("remind_before_number", xiCollection["remind_before_number"]);
            Parameter param4 = new Parameter("remind_before_text", xiCollection["remind_before_text"]);
            Parameter param5 = new Parameter("appointmentid", xiCollection["appointmentid"], DbType.Int32);
            Parameter param6 = new Parameter("dateid", xiId, DbType.Int32);


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateAnimalAppointmentToDo(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update animal_appointment_dates set todo_text=@todo_text,resultsandmedication=@resultsandmedication,filenames=@filenames where active=1 and id=@id";

            Parameter param1 = new Parameter("todo_text", xiCollection["todo_text"]);
            Parameter param2 = new Parameter("resultsandmedication", xiCollection["resultsandmedication"]);
            Parameter param3 = new Parameter("filenames", xiCollection["filenames"]);
            Parameter param4 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.TODOAPPOINTMENT.ToString();
            logCollection["category"] = Common.AnimalLogCategory.APPOINTMENTS.ToString();
            logCollection["description"] = (xiCollection["todo_text"].Length > 100) ? xiCollection["todo_text"].Substring(0, 100) : xiCollection["todo_text"];
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public bool CloseAnimalAppointmentDate(object xiAppointmentDateId)
        {
            string query = "update animal_appointment_dates set status=0 where active=1 and id=@id and status=1";

            Parameter param4 = new Parameter("id", xiAppointmentDateId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetAnimalAppointmentCount(string xiFilter, object xiUserId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(a.id) as maxcount from animal_appointment a
inner join animal_appointment_dates ad on ad.appointmentid=a.id and ad.active = 1 and ad.[status]=1
inner join view_animal ani on ani.id  = a.animalid inner join user_animal ua on ua.animalid = ani.id and ua.userid = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + @"
inner join contact c on c.id = a.contactid 
left join contact_servicetype cs on cs.id = c.service_type  
where a.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalAppointmentCount", x.Message);
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

        public static DataSet GetAnimalAppointmentDetails(int xiPage, string xiFilter, object xiUserId)
        {
            string query = string.Empty;
            if (xiPage == -1)
            {
                query = @"select top 100 a.id, a.contactid, ad.id as dateid, ad.startdatetime,
(c.fname + ' ' + c.lname) as contact_name, isnull(cs.[name], '(None)') as profession_name,a.animalid,
ani.name as animal_name, ani.animalcategory, ani.typename, ani.profilepic_file, ani.breedimage, ua.userid, c.email as email, c.phone as phone,a.bu_id
from animal_appointment a
inner join animal_appointment_dates ad on ad.appointmentid=a.id and ad.active = 1 and ad.[status]=1
inner join view_animal ani on ani.id  = a.animalid inner join user_animal ua on ua.animalid = ani.id and ua.userid = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + @"
inner join contact c on c.id = a.contactid 
left join contact_servicetype cs on cs.id = c.service_type  
where a.active=1";
                if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
                query += " order by ad.startdatetime";
            }
            else
            {

                xiPage = (xiPage <= 0) ? 1 : xiPage;
                xiPage = xiPage - 1;

                query = @"select a.id, a.contactid, ad.id as dateid, ad.startdatetime, ad.[status],
(c.fname + ' ' + c.lname) as contact_name, isnull(cs.[name], '(None)') as profession_name,a.animalid,
ani.name as animal_name, ani.typename, ani.animalcategory, ani.profilepic_file, ani.breedimage, ua.userid, c.email as email, c.phone as phone,a.bu_id
from animal_appointment a
inner join animal_appointment_dates ad on ad.appointmentid=a.id and ad.active = 1
inner join view_animal ani on ani.id  = a.animalid inner join user_animal ua on ua.animalid = ani.id and ua.userid = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + @"
inner join contact c on c.id = a.contactid 
left join contact_servicetype cs on cs.id = c.service_type  
where a.active=1";
                if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
                query += " order by ad.startdatetime offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            }
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                ds.Tables[0].Columns.Add("procesed_date", typeof(string));
                ds.Tables[0].Columns.Add("procesed_time", typeof(string));
                ds.Tables[0].Columns.Add("procesed_datetime", typeof(string));
                ds.Tables[0].Columns.Add("ispastappointment", typeof(Int16));

                DateTime now = BusinessBase.Now;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["startdatetime"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_datetime"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                        row["procesed_date"] = tempDate1.ToString(BusinessBase.DateFormat);
                        row["procesed_time"] = tempDate1.ToString("HH:mm");
                        row["ispastappointment"] = (tempDate1 < now) ? 1 : 0;
                    }
                }

            }
            return ds;
        }

        public static bool DeleteAnimalAppointment(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update animal_appointment set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = BusinessBase.ConvertToString(xiId);
            logCollection["key"] = Common.AnimalLogKey.DELETEAPPOINTMENT.ToString();
            logCollection["category"] = Common.AnimalLogCategory.APPOINTMENTS.ToString();
            logCollection["description"] = null; // Appointment is booked from 10/07/2023 - 30/07/2023 for ABCAnimal
            logCollection["userid"] = "1"; // Admin
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public static NameValueCollection GetAnimalAppointmentDetaillByDate(object xiAppointmentDateId, string xiUserId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select a.*, ad.id as dateid, ad.startdatetime, ad.[status], ad.todo_text, ad.resultsandmedication, ad.filenames, (c.fname + ' ' + c.lname) as contact_name, cs.id as professionid,isnull(cs.[name], '(None)') as profession_name, c.email as email, c.phone as phone,
ani.name as animal_name, ani.typename, ua.userid from animal_appointment a
inner join animal_appointment_dates ad on ad.appointmentid=a.id and ad.active = 1
inner join view_animal ani on ani.id  = a.animalid inner join user_animal ua on ua.animalid = ani.id and ua.userid = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + @"
inner join contact c on c.id = a.contactid left join contact_servicetype cs on cs.id = c.service_type  
where a.active=1 and ad.id = " + Utils.ConvertToDBString(xiAppointmentDateId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("dateid", Convert.ToString(objdb.GetValue(reader1, "dateid")));
                    collection.Add("appointmentid", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("startdatetime", Convert.ToString(objdb.GetValue(reader1, "startdatetime")));
                    collection.Add("animalid", Convert.ToString(objdb.GetValue(reader1, "animalid")));
                    collection.Add("contactid", Convert.ToString(objdb.GetValue(reader1, "contactid")));
                    collection.Add("todo_text", Convert.ToString(objdb.GetValue(reader1, "todo_text")));
                    collection.Add("resultsandmedication", Convert.ToString(objdb.GetValue(reader1, "resultsandmedication")));
                    collection.Add("filenames", Convert.ToString(objdb.GetValue(reader1, "filenames")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("remind_before_text", Convert.ToString(objdb.GetValue(reader1, "remind_before_text")));
                    collection.Add("remind_before_number", Convert.ToString(objdb.GetValue(reader1, "remind_before_number")));
                    collection.Add("contact_name", Convert.ToString(objdb.GetValue(reader1, "contact_name")));
                    collection.Add("professionid", Convert.ToString(objdb.GetValue(reader1, "professionid")));
                    collection.Add("profession_name", Convert.ToString(objdb.GetValue(reader1, "profession_name")));
                    collection.Add("contact_email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("contact_phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("animal_name", Convert.ToString(objdb.GetValue(reader1, "animal_name")));
                    collection.Add("typename", Convert.ToString(objdb.GetValue(reader1, "typename")));
                    collection.Add("userid", Convert.ToString(objdb.GetValue(reader1, "userid")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalAppointmentDetail", x.Message);
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

        public static DataTable GetContactsByProfession(object xiProfessionId, object xiUserId)
        {
            string query = "select id, (fname +' '+ lname) as full_name from contact where active = 1 and service_type=" + Utils.ConvertToDBString(xiProfessionId, Utils.DataType.Integer) + " and createdby=" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " order by fname, lname";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static bool AddAnimalAppointment_Files(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [animal_appointment_files]([appointmentid],[file]) values(@appointmentid,@file)";
            Parameter param1 = new Parameter("appointmentid", xiCollection["appointmentid"], DbType.Int32);
            Parameter param2 = new Parameter("file", xiCollection["file"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetdAnimalAppointmentFiles(object xiAppointmentId)
        {
            string query = "select * from animal_appointment_files where appointmentid =" + Utils.ConvertToDBString(xiAppointmentId, Utils.DataType.Integer) + " order by [file]";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string AppointmentSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["animalid"] != null && xiCollection["animalid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.animalid={0})", Utils.ConvertToDBString(xiCollection["animalid"], Utils.DataType.Integer)));
            }

            if (xiCollection["category"] != null && xiCollection["category"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.service_type={0})", Utils.ConvertToDBString(xiCollection["category"], Utils.DataType.Integer)));
            }

            if (xiCollection["contact"] != null && xiCollection["contact"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.fname + ' ' + c.lname like {0} or c.lname + ' ' + c.fname like {0})", Utils.ConvertToDBString("%" + xiCollection["contact"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["description"] != null && xiCollection["description"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(ad.[resultsandmedication] like {0})", Utils.ConvertToDBString("%" + xiCollection["description"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["category"] != null && xiCollection["category"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.service_type={0})", Utils.ConvertToDBString(xiCollection["category"], Utils.DataType.Integer)));
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
                    builder.Append(string.Format("(ad.startdatetime between {0} and {1})", startdate, enddate));
                }
                catch { }
            }
            //todo new chng
            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.bu_id is null or a.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static bool DeleteThisAppointment(object xiAppointmentDateId)
        {
            string query = string.Format("update animal_appointment_dates set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiAppointmentDateId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool DeleteThisAndFollowingAppointment(object xiAppointmentDateId)
        {
            string query = "update animal_appointment_dates set active = 0 where id >= @id and appointmentid = (select iped.appointmentid from animal_appointment_dates iped where iped.id = @id)";

            Parameter param1 = new Parameter("id", xiAppointmentDateId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool DeleteAllAppointments(object xiAppointmentId)
        {
            string query = @"update animal_appointment set active = 0 where id = @appid;update animal_appointment_dates set active = 0 where appointmentid = @appid;";
            Parameter param1 = new Parameter("appid", xiAppointmentId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();
            return (value > 0);
        }

        public static bool DeleteAppointmentPhoto(string xiFileName)
        {
            string query = "delete from [animal_appointment_files] where [file] = @file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();

            return (value > 0);

        }

        #endregion

        #region Animal_Transfer

        public bool AddAnimalTransfer(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [dbo].[animal_transfer]([animalid],[userid_old],[userid_new],[transfer_date],[files],[status],[active],[submitdate]) values(@animalid,@userid_old,@userid_new,@transfer_date,@files,0,1,getutcdate())";
            Parameter param1 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
            Parameter param2 = new Parameter("userid_old", xiCollection["userid_old"], DbType.Int32);
            Parameter param3 = new Parameter("userid_new", xiCollection["userid_new"], DbType.Int32);
            Parameter param4 = new Parameter("transfer_date", xiCollection["transfer_date"], DbType.Date);
            Parameter param6 = new Parameter("files", xiCollection["files"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param6 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = xiCollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.TRANSFERREQUESTSEND.ToString();
            logCollection["category"] = Common.AnimalLogCategory.TRANSFER.ToString();
            logCollection["description"] = xiCollection["email"];
            logCollection["userid"] = xiCollection["userid"];
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public static bool IsPendingTransferRequest(object xiAnimalId)
        {
            string query = "select 1 from animal_transfer where animalid =" + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer) + " and [status] = 0";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object obj = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return (BusinessBase.ConvertToInteger(obj) > 0);
        }

        public static int GetAnimalTransferCount(string xiFilter, object xiUserId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(t.id) as maxcount from animal_transfer t inner join [user] u1 on t.userid_old = u1.id 
inner join [user] u2 on t.userid_new = u2.id  inner join view_animal a on t.animalid = a.id where t.active=1 and t.userid_new = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalTransferCount", x.Message);
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

        public static DataSet GetAnimalTransferDetails(int xiPage, string xiFilter, object xiUserId)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;
            string query = @"select t.*, (u1.fname + ' ' + u1.lname) as sender_full_name, (u2.fname + ' ' + u2.lname) as receiver_full_name,a.name as animalname, a.typename , a.profilepic_file, a.breedimage
from animal_transfer t inner join [user] u1 on t.userid_old = u1.id inner join [user] u2 on t.userid_new = u2.id inner join view_animal a on t.animalid = a.id
where t.active=1 and t.userid_new = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by t.[status], t.submitdate offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_transferdate", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                    DateTime tempDate2 = Convert.ToDateTime(row["transfer_date"]);
                    if (tempDate1 != DateTime.MinValue && tempDate2 != DateTime.MinValue)
                    {
                        row["procesed_date"] = tempDate1.ToString(BusinessBase.DateFormat);
                        row["processed_transferdate"] = tempDate2.ToString(BusinessBase.DateFormat);
                    }
                }
            }

            return ds;
        }

        public static NameValueCollection GetAnimalTransferDetail(object xiTransferId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select t.*,u.fname,u.lname,u.email from animal_transfer t  inner join [user] as u on t.userid_new = u.id where t.active=1 and t.id =" + Utils.ConvertToDBString(xiTransferId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("transfer_date", Convert.ToString(objdb.GetValue(reader1, "transfer_date")));
                    collection.Add("animalid", Convert.ToString(objdb.GetValue(reader1, "animalid")));
                    collection.Add("userid_old", Convert.ToString(objdb.GetValue(reader1, "userid_old")));
                    collection.Add("userid_new", Convert.ToString(objdb.GetValue(reader1, "userid_new")));
                    collection.Add("files", Convert.ToString(objdb.GetValue(reader1, "files")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));

                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalTransferDetails", x.Message);
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

        public static bool UpdateAnimalTransferApprove(object xiTransferId, string xiUserId)
        {
            NameValueCollection transfercollection = AnimalBA.GetAnimalTransferDetail(xiTransferId);
            if (transfercollection == null) return false;

            string query = "update animal_transfer set status=1 where id=@id and [status]=0 and  active=1 and userid_new=@userid";
            Parameter param1 = new Parameter("id", xiTransferId, DbType.Int32);
            Parameter param2 = new Parameter("userid", transfercollection["userid_new"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            if (value > 0)
            {
                string query1 = "update user_animal set userid=@userid where animalid=@animalid";
                Parameter param3 = new Parameter("animalid", transfercollection["animalid"], DbType.Int32);
                objdb.ExecuteNonQuery(objdb.con, query1, new Parameter[] { param2, param3 });
            }
            objdb.Disconnectdb();

            string newownername = transfercollection["fname"] + " " + transfercollection["lname"];

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = transfercollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.TRANSFERREQUESTACCEPT.ToString();
            logCollection["category"] = Common.AnimalLogCategory.TRANSFER.ToString();
            logCollection["description"] = transfercollection["email"]; //newownername; //transfercollection["email"] + "*" +
            logCollection["userid"] = xiUserId;
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public static bool UpdateAnimalTransferReject(object xiTransferId, string xiUserId)
        {
            NameValueCollection transfercollection = AnimalBA.GetAnimalTransferDetail(xiTransferId);
            if (transfercollection == null) return false;

            string query = "update animal_transfer set status=2 where id=@id and [status]=0 and  active=1 and userid_new=@userid";
            Parameter param1 = new Parameter("id", xiTransferId, DbType.Int32);
            Parameter param2 = new Parameter("userid", transfercollection["userid_new"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            string newownername = transfercollection["fname"] + " " + transfercollection["lname"];

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = transfercollection["animalid"];
            logCollection["key"] = Common.AnimalLogKey.TRANSFERREQUESTREJECT.ToString();
            logCollection["category"] = Common.AnimalLogCategory.TRANSFER.ToString();
            logCollection["description"] = transfercollection["email"]; //newownername; // transfercollection["email"] + "*" +
            logCollection["userid"] = xiUserId;
            Common.SaveAnimalLog(logCollection);

            return (value > 0);
        }

        public static string TransferSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["status"] != null && xiCollection["status"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(t.[status]={0})", Utils.ConvertToDBString(xiCollection["status"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }


        #endregion

        #region Animal_Custom_Fields

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

        public static DataTable GetCustomFields(string xiBreedType)
        {
            return GetCustomFields(xiBreedType, -1);
        }

        public static DataTable GetCustomFields(string xiBreedType, object xiAnimalId)
        {
            string query = @"select c.*,cv.fieldvalue,(select stuff(( 
select ', ' + co.optiontext from animal_customfieldsoption co where co.id in
(select value from animal_customfields_values cv1
CROSS APPLY STRING_SPLIT(SUBSTRING (cv1.fieldvalue ,0 ,LEN(cv1.fieldvalue)+100), ',') where cv1.id = cv.id and cv.fieldid in (select id from animal_customfields where [type] in (3,4,5)))
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as optiontext
from animal_customfields c 
left join animal_customfields_values cv on cv.fieldid = c.id and cv.animalid = " + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer) + @"
where c.active = 1 and (c.breedtype is null or ','+c.breedtype+',' like " + Utils.ConvertToDBString("%," + xiBreedType + ",%", Utils.DataType.String) + ") order by c.sortorder,c.title";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetBUCustomFields(string xiBreedType, object xiAnimalId, string xiBUID)
        {
            string query = @"select c.*,cv.fieldvalue,(select stuff(( 
select ', ' + co.optiontext from animal_customfieldsoption co where co.id in
(select value from animal_customfields_values cv1
CROSS APPLY STRING_SPLIT(SUBSTRING (cv1.fieldvalue ,0 ,LEN(cv1.fieldvalue)+100), ',') where cv1.id = cv.id and cv.fieldid in (select id from animal_customfields where [type] in (3,4,5)))
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as optiontext
from animal_customfields c 
left join animal_customfields_values cv on cv.fieldid = c.id and cv.animalid = " + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer) + @"
where (c.breedtype is null or ','+c.breedtype+',' like " + Utils.ConvertToDBString("%," + xiBreedType + ",%", Utils.DataType.String) + ") and c.bu_id is null or c.bu_id=" + Utils.ConvertToDBString(xiBUID, Utils.DataType.Integer) + " and c.active = 1 order by c.sortorder,c.title";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static void SaveCustomFields(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return;

            string query = @"if exists(select 1 from animal_customfields_values where fieldid = @field and animalid = @animalid) update animal_customfields_values set fieldvalue = @val where fieldid = @field and animalid = @animalid
		    else insert into animal_customfields_values([fieldid],[animalid],[fieldvalue]) values (@field, @animalid,@val)";

            Parameter param1 = new Parameter("field", xiCollection["fieldid"], DbType.Int32);
            Parameter param2 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
            Parameter param4 = new Parameter("val", xiCollection["fieldvalue"], DbType.String);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param4 });
            objdb.Disconnectdb();
        }

        public static DataTable GetCustomFieldsOptions(object xiCustomFieldId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select id, optiontext from animal_customfieldsoption where fieldid = " + Utils.ConvertToDBString(xiCustomFieldId, Utils.DataType.Integer);
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static void AdjustOptionKeys(DataTable xiDataTable)
        {
            if (xiDataTable == null || xiDataTable.Rows.Count == 0) return;

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < xiDataTable.Rows.Count; i++)
            {
                DataRow row = xiDataTable.Rows[i];
                builder.Append(string.Format("update animal_customfieldsoption set optionkey={0} where id={1};", Utils.ConvertToDBString("ctl00$ContentPlaceHolder1$txtColumnVal" + (i + 1), Utils.DataType.String), Utils.ConvertToDBString(row["id"], Utils.DataType.Integer)));
            }

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            objdb.ExecuteNonQuery(objdb.con, builder.ToString());
            objdb.Disconnectdb();
            builder = null;
        }

        public static int GetColumnId(object xiFieldId, string xiKeyValue)
        {
            string query = @"select id from animal_customfieldsoption where fieldid = " + Utils.ConvertToDBString(xiFieldId, Utils.DataType.Integer) + " and optionkey = " + Utils.ConvertToDBString(xiKeyValue, Utils.DataType.String);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object retVal = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return BusinessBase.ConvertToInteger(retVal);
        }


        #endregion

        #region Manage Breed

        public bool AddBreedCategory(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into animal_categary(breedname,breedimage,active)values(@breedname,@breedimage,1)";

            Parameter param1 = new Parameter("breedname", xiCollection["breedname"]);
            Parameter param2 = new Parameter("breedimage", xiCollection["breedimage"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateBreedCategory(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update animal_categary set breedname=@breedname,breedimage=@breedimage where id=@id";

            Parameter param1 = new Parameter("breedname", xiCollection["breedname"]);
            Parameter param2 = new Parameter("breedimage", xiCollection["breedimage"]);
            Parameter param3 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        /// <summary>
        /// Type Of Animal like DOG, Cat Horse
        /// </summary>
        /// <param name="xiId"></param>
        /// <returns></returns>
        public static NameValueCollection GetBreedCategory(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,breedname,breedimage from animal_categary where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("breedname", Convert.ToString(objdb.GetValue(reader1, "breedname")));
                    collection.Add("breedimage", Convert.ToString(objdb.GetValue(reader1, "breedimage")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBreedDoc", x.Message);
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

        /// <summary>
        /// like:
        /// Dog - German shephard
        /// Horse - Arabian
        /// </summary>
        /// <param name="xiId"></param>
        /// <returns></returns>
        public static string DeleteBreedCategory(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from animal_category_type where categoryid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteBreedCategory", x.Message);
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
                query = string.Format("update animal_category_type set active = 0 where id= {0}", xiId);
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

        public static DataSet GetBreedCategories(string xiFilter)
        {
            string query = "select t.id,t.breedname,t.breedimage from animal_categary t where t.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by t.breedname";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        #endregion

        #region Manage Breed Type

        public int AddBreedType(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "insert into animal_category_type(name,categoryid,active)values(@name,@categoryid,1)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("categoryid", xiCollection["categoryid"], DbType.Int32);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public bool UpdateBreedType(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update animal_category_type set name=@name,categoryid=@categoryid where id=@id and active = 1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("categoryid", xiCollection["categoryid"], DbType.Int32);
            Parameter param3 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetBreedType(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,categoryid from animal_category_type where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("categoryid", Convert.ToString(objdb.GetValue(reader1, "categoryid")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBreedType", x.Message);
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

        public static NameValueCollection GetBreedTypeName(object xiName)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,categoryid from animal_category_type where active=1 and name=" + Utils.ConvertToDBString(xiName, Utils.DataType.String);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("categoryid", Convert.ToString(objdb.GetValue(reader1, "categoryid")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBreedType", x.Message);
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

        //public static string DeleteBreedCategory(object xiId)
        //{
        //    DBClass objdb = new DBClass();
        //    objdb.Connectdb();
        //    int totalCount = 0;
        //    SqlDataReader reader1 = null;
        //    string returnValue = string.Empty;
        //    string query = "select Count(*) as maxcount from animal where  breedtype=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
        //    try
        //    {
        //        reader1 = objdb.ExecuteReader(objdb.con, query);
        //        while (reader1.Read())
        //        {
        //            totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
        //            break;
        //        }
        //    }
        //    catch (Exception x)
        //    {
        //        objdb.Write_log_file("DeleteBreedCategory", x.Message);
        //    }
        //    finally
        //    {
        //        if (reader1 != null)
        //        {
        //            reader1.Close();
        //            reader1 = null;
        //        }
        //    }
        //    if (totalCount == 0)
        //    {
        //        query = string.Format("update animal_category_type set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));
        //        int value = objdb.ExecuteNonQuery(objdb.con, query);
        //        objdb.Disconnectdb();
        //        returnValue = (value > 0) ? "Record deleted." : "false";
        //        return returnValue;
        //    }
        //    else
        //    {
        //        returnValue = "You can not delete this item because its already used.";
        //    }
        //    return returnValue;
        //}

        //public static string DeleteBreedType(int xiId)
        //{
        //    DBClass objdb = new DBClass();
        //    objdb.Connectdb();
        //    string returnValue = string.Empty;
        //    string query = string.Format("update animal_category_type set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));
        //    objdb.Connectdb();

        //    int value = objdb.ExecuteNonQuery(objdb.con, query);
        //    objdb.Disconnectdb();

        //    returnValue = (value > 0) ? "Record deleted" : "false";
        //    return returnValue;
        //}

        public static DataSet GetBreedTypes(string xiFilter)
        {
            string query = "select t.id,t.name,t.categoryid,ac.breedname from animal_category_type t inner join animal_categary ac on ac.id=t.categoryid where t.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by t.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataTable GetBreedType()
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from [animal_categary] where active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        /// <summary>
        /// like: 
        /// Dog
        /// Horse
        /// </summary>
        /// <param name="xiId"></param>
        /// <returns></returns>
        public static string DeleteAnimalCategory(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select Count(*) as maxcount from animal where animalcategory =" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteAnimalCategory", x.Message);
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
                string query2 = @"Update animal_category_type set active = 0 where categoryid = {0}; Update animal_categary set active = 0 where id = {0};";
                query2 = string.Format(query2, Utils.ConvertToDBString(xiId, Utils.DataType.Integer));
                int value = objdb.ExecuteNonQuery(objdb.con, query2);
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

        #endregion

        #region Manage Profession

        public bool AddProfession(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into contact_servicetype(name,active,bu_id)values(@name,1,@companyid)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("companyid", xiCollection["companyid"]);


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateProfession(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update contact_servicetype set name=@name where id=@id";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetProfession(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from contact_servicetype where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetProfession", x.Message);
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

        public static string DeleteProfession(int xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from contact where service_type=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteProfession", x.Message);
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
                query = string.Format("update contact_servicetype set active = 0 where id= {0}", xiId);
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

        public static DataSet GetProfessions(string xiFilter)
        {
            string query = "select t.id,t.name,t.bu_id from contact_servicetype t where t.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and (bu_id is null or bu_id=" + Utils.ConvertToDBString(xiFilter, Utils.DataType.Integer) + ")";
            query += " order by t.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        //public static DataSet GetBUProfessions(string xiFilter)
        //{
        //    string query = "select t.id,t.name,t.bu_id from contact_servicetype t where t.active = 1";
        //    if (string.IsNullOrEmpty(xiFilter) == false) query += " and bu_id is null or bu_id=" + xiFilter;
        //    query += " order by t.name";

        //    DBClass objdb = new DBClass();
        //    objdb.Connectdb();
        //    DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
        //    objdb.Disconnectdb();

        //    return ds;
        //}

        #endregion

        #region Report

        public static string AllAnimalReportSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["animalname"] != null && xiCollection["animalname"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.name like {0})", Utils.ConvertToDBString("%" + xiCollection["animalname"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["username"] != null && xiCollection["username"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(u.fname + ' ' + u.lname like {0} or u.lname + ' ' + u.fname like {0})", Utils.ConvertToDBString("%" + xiCollection["username"] + "%", Utils.DataType.String)));
            }

            if (BusinessBase.ConvertToInteger(xiCollection["usertype"]) > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(u.[type]={0})", Utils.ConvertToDBString(xiCollection["usertype"], Utils.DataType.Integer)));
            }

            if (BusinessBase.ConvertToInteger(xiCollection["animalcategory"]) > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.animalcategory={0})", Utils.ConvertToDBString(xiCollection["animalcategory"], Utils.DataType.Integer)));
            }

            if (xiCollection["breedtypes"] != null && xiCollection["breedtypes"].Length > 0)
            {
                string[] types = xiCollection["breedtypes"].Split(',');
                if (types != null && types.Length > 0)
                {
                    StringBuilder statusBuilder = new StringBuilder();
                    statusBuilder.Append("(");
                    bool isOr = false;
                    foreach (string ty in types)
                    {
                        if (isOr) statusBuilder.Append(" or ");
                        isOr = true;
                        statusBuilder.Append("(a.breedtype = " + Utils.ConvertToDBString(ty, Utils.DataType.Integer) + ")");
                        break;
                    }
                    statusBuilder.Append(")");

                    if (iswhere) builder.Append(" and ");
                    iswhere = true;
                    builder.Append(statusBuilder.ToString());
                    statusBuilder = null;
                }
            }
            return builder.ToString();
        }

        public static int GetAllAnimalReportCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(a.id) as maxcount from view_animal a inner join user_animal ua on a.id=ua.animalid
inner join [user] u on ua.userid= u.id and u.active=1 where a.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAllAnimalReportCount", x.Message);
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

        public static DataSet GetAllAnimalReport(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"select a.id, a.name, a.profilepic_file , a.typename, a.categoryname, a.dob, a.breedimage,(u.fname + ' ' + u.lname) as username, u.email as email, u.phone as phone,  u.[type]
from view_animal a inner join user_animal ua on a.id=ua.animalid
inner join [user] u on ua.userid= u.id and u.active=1 where a.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by username, a.name offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_dob", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["dob"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_dob"] = tempDate1.ToString(BusinessBase.DateFormat);
                    }
                }
            }

            return ds;
        }

        #endregion

        #region Animal Log

        public static int GetAnimalLogCount(string xiFilter, object xiUserId)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(l.id) as maxcount from animal_log l 
inner join animal a on l.animal_id = a.id inner join animal_categary c on c.id = a.animalcategory and c.active = 1
inner join [user] u on u.id = l.submitby where a.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAnimalLogCount", x.Message);
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

        public static DataSet GetAnimalLogDetails(int xiPage, string xiFilter, object xiUserId)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"select l.*, a.[name],a.id,a.mothername, a.fathername, a.animalcategory, a.breedtype,c.breedname as 'animaltype',(u.fname + ' '+u.lname) as username
from animal_log l inner join animal a on l.animal_id = a.id
inner join animal_categary c on c.id = a.animalcategory and c.active = 1
inner join [user] u on u.id = l.submitby
where a.active = 1";

            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by l.submitdate desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_datetime", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                    string logdatetime = string.Empty;
                    if (tempDate1 != DateTime.MinValue)
                    {
                        logdatetime = tempDate1.ToString(BusinessBase.DateTimeFormat);

                    }
                    row["procesed_datetime"] = logdatetime;
                }
            }

            return ds;
        }

        public static string AnimalLogSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["animalname"] != null && xiCollection["animalname"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.[name] like {0})", Utils.ConvertToDBString("%" + xiCollection["animalname"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["animalid"] != null && xiCollection["animalid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.id={0})", Utils.ConvertToDBString(xiCollection["animalid"], Utils.DataType.Integer)));
            }


            if (xiCollection["category"] != null && xiCollection["category"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(l.category={0})", Utils.ConvertToDBString(xiCollection["category"], Utils.DataType.String)));
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
                    builder.Append(string.Format("(l.submitdate between {0} and {1})", startdate, enddate));
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

        #endregion

        #region Animal Checklist

        public bool AssignChecklist(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "if not exists(select 1 from animal_checklist where checklistid=@checklistid and animalid=@animalid) insert into animal_checklist(checklistid,animalid) values(@checklistid, @animalid)";

            Parameter param1 = new Parameter("checklistid", xiCollection["checklistid"], DbType.Int32);
            Parameter param2 = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);

            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);

        }

        public static bool UnassignedChecklist(object xiId)
        {
            string query = "delete from animal_checklist where id=@id";

            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetAnimalChecklist(object xiAnimalId)
        {
            string query = @"select ac.id, ac.checklistid, c.name, cat.name as categoryname from animal_checklist ac inner join checklist c on ac.checklistid = c.id  
left join checklist_category cat on cat.id = c.category and cat.active =1 where c.active = 1 and ac.animalid = " + Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        #endregion

        //new 20 dec 2023 nilesh
        #region Animal Type Class
        public int AddTypeClass(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "insert into animal_class_type(name,categoryid,active)values(@name,@categoryid,1)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("categoryid", xiCollection["categoryid"], DbType.Int32);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public bool UpdateTypeClass(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update animal_class_type set name=@name,categoryid=@categoryid where id=@id and active = 1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("categoryid", xiCollection["categoryid"], DbType.Int32);
            Parameter param3 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetTypeClass(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,categoryid from animal_class_type where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("categoryid", Convert.ToString(objdb.GetValue(reader1, "categoryid")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetTypeClass", x.Message);
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

        public static NameValueCollection GetTypeClassName(object xiName)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,categoryid from animal_class_type where active=1 and name=" + Utils.ConvertToDBString(xiName, Utils.DataType.String);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("categoryid", Convert.ToString(objdb.GetValue(reader1, "categoryid")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetTypeClassName", x.Message);
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

        public static DataSet GetTypeClassDetails(string xiFilter)
        {
            string query = "select ct.id,ct.name,ct.categoryid,ac.breedname from animal_class_type ct inner join animal_categary ac on ac.id=ct.categoryid where ct.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by ct.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataTable GetTypeClass()
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from [animal_class_type] where active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string DeleteTypeClass(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from animal where classtypeid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteTypeClass", x.Message);
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
                query = string.Format("update animal_class_type set active = 0 where id= {0}", xiId);
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
        public static DataTable GetAnimalTypeClass(string xiCategoryId) // Here we need animalid 
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select ct.id,ct.name from animal_class_type ct inner join [animal] a on ct.categoryid = a.animalcategory where a.id = " + Utils.ConvertToDBString(xiCategoryId, Utils.DataType.Integer) + " and ct.active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        #endregion
    }
}
