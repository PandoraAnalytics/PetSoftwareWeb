using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BABusiness
{
    public class Member : BusinessBase
    {
        #region Member

        public int AddMember(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            if (!xiCollection.AllKeys.Contains("active") && string.IsNullOrEmpty(xiCollection["active"])) xiCollection["active"] = "1";

            string query = @"INSERT INTO [dbo].[member] (memberno,fname,lname,street,address,city,country,zipcode,email,mobile,phone,fax,entrydate,exitdate,exitreason,
membershiptype,familyfullmember,region,positioninregion,isbreeder,familymemberof,remark,paymentmethod,accountnumber,accountownername,bankname,
bankcode,breedtype,animalname,animalbirthdate,collarid,submitdate,active,submitby)VALUES(";

            query += Utils.ConvertToDBString(xiCollection["memberno"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["fname"], Utils.DataType.Encrypted) + ", ";
            query += Utils.ConvertToDBString(xiCollection["lname"], Utils.DataType.Encrypted) + ", ";
            query += Utils.ConvertToDBString(xiCollection["street"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["address"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["city"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["country"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["zipcode"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["email"], Utils.DataType.Encrypted) + ", ";
            query += Utils.ConvertToDBString(xiCollection["mobile"], Utils.DataType.Encrypted) + ", ";
            query += Utils.ConvertToDBString(xiCollection["phone"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["fax"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["entrydate"], Utils.DataType.Date) + ", ";
            query += Utils.ConvertToDBString(xiCollection["exitdate"], Utils.DataType.Date) + ", ";
            query += Utils.ConvertToDBString(xiCollection["exitreason"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["membershiptype"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["familyfullmember"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["region"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["positioninregion"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["isbreeder"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["familymemberof"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["remark"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["paymentmethod"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["accountnumber"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["accountownername"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["bankname"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["bankcode"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["breedtype"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["animalname"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["animalbirthdate"], Utils.DataType.Date) + ", ";
            query += Utils.ConvertToDBString(xiCollection["collarid"], Utils.DataType.String) + ", ";
            query += "getutcdate(), ";
            query += Utils.ConvertToDBString(xiCollection["active"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["submitby"], Utils.DataType.Integer);
            query += ");";

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2);
            objdb.Disconnectdb();
            return (value > 0) ? value : int.MinValue;

        }

        public static NameValueCollection GetMember(object xiMemberId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select m.*,am.association_id from member m inner join [association_members] am on m.id = am.memberid  where m.active = 1 and m.id = " + Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("memberno", Convert.ToString(objdb.GetValue(reader1, "memberno")));
                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("street", Convert.ToString(objdb.GetValue(reader1, "street")));
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    collection.Add("city", Convert.ToString(objdb.GetValue(reader1, "city")));
                    collection.Add("country", Convert.ToString(objdb.GetValue(reader1, "country")));
                    collection.Add("zipcode", Convert.ToString(objdb.GetValue(reader1, "zipcode")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("mobile", Convert.ToString(objdb.GetValue(reader1, "mobile")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("fax", Convert.ToString(objdb.GetValue(reader1, "fax")));
                    collection.Add("entrydate", Convert.ToString(objdb.GetValue(reader1, "entrydate")));
                    collection.Add("exitdate", Convert.ToString(objdb.GetValue(reader1, "exitdate")));
                    collection.Add("exitreason", Convert.ToString(objdb.GetValue(reader1, "exitreason")));
                    collection.Add("membershiptype", Convert.ToString(objdb.GetValue(reader1, "membershiptype")));
                    collection.Add("familyfullmember", Convert.ToString(objdb.GetValue(reader1, "familyfullmember")));
                    collection.Add("region", Convert.ToString(objdb.GetValue(reader1, "region")));
                    collection.Add("positioninregion", Convert.ToString(objdb.GetValue(reader1, "positioninregion")));
                    collection.Add("isbreeder", Convert.ToString(objdb.GetValue(reader1, "isbreeder")));
                    collection.Add("familymemberof", Convert.ToString(objdb.GetValue(reader1, "familymemberof")));
                    collection.Add("remark", Convert.ToString(objdb.GetValue(reader1, "remark")));
                    collection.Add("paymentmethod", Convert.ToString(objdb.GetValue(reader1, "paymentmethod")));
                    collection.Add("accountnumber", Convert.ToString(objdb.GetValue(reader1, "accountnumber")));
                    collection.Add("accountownername", Convert.ToString(objdb.GetValue(reader1, "accountownername")));
                    collection.Add("bankname", Convert.ToString(objdb.GetValue(reader1, "bankname")));
                    collection.Add("bankcode", Convert.ToString(objdb.GetValue(reader1, "bankcode")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("animalname", Convert.ToString(objdb.GetValue(reader1, "animalname")));
                    collection.Add("animalbirthdate", Convert.ToString(objdb.GetValue(reader1, "animalbirthdate")));
                    collection.Add("collarid", Convert.ToString(objdb.GetValue(reader1, "collarid")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("submitby", Convert.ToString(objdb.GetValue(reader1, "submitby")));
                    collection.Add("association_id", Convert.ToString(objdb.GetValue(reader1, "association_id")));



                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetMember", x.Message);
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

        public bool UpdateMember(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update member set ";
            query += "memberno = " + Utils.ConvertToDBString(xiCollection["memberno"], Utils.DataType.String) + ", ";
            query += "fname = " + Utils.ConvertToDBString(xiCollection["fname"], Utils.DataType.Encrypted) + ", ";
            query += "lname = " + Utils.ConvertToDBString(xiCollection["lname"], Utils.DataType.Encrypted) + ", ";
            query += "street = " + Utils.ConvertToDBString(xiCollection["street"], Utils.DataType.String) + ", ";
            query += "address = " + Utils.ConvertToDBString(xiCollection["address"], Utils.DataType.String) + ", ";
            query += "city = " + Utils.ConvertToDBString(xiCollection["city"], Utils.DataType.String) + ", ";
            query += "country = " + Utils.ConvertToDBString(xiCollection["country"], Utils.DataType.String) + ", ";
            query += "zipcode = " + Utils.ConvertToDBString(xiCollection["zipcode"], Utils.DataType.String) + ", ";
            query += "email = " + Utils.ConvertToDBString(xiCollection["email"], Utils.DataType.Encrypted) + ", ";
            query += "mobile = " + Utils.ConvertToDBString(xiCollection["mobile"], Utils.DataType.Encrypted) + ", ";
            query += "phone = " + Utils.ConvertToDBString(xiCollection["phone"], Utils.DataType.String) + ", ";
            query += "fax = " + Utils.ConvertToDBString(xiCollection["fax"], Utils.DataType.String) + ", ";
            query += "entrydate = " + Utils.ConvertToDBString(xiCollection["entrydate"], Utils.DataType.Date) + ", ";
            query += "exitdate = " + Utils.ConvertToDBString(xiCollection["exitdate"], Utils.DataType.Date) + ", ";
            query += "exitreason = " + Utils.ConvertToDBString(xiCollection["exitreason"], Utils.DataType.String) + ", ";
            query += "membershiptype = " + Utils.ConvertToDBString(xiCollection["membershiptype"], Utils.DataType.String) + ", ";
            query += "familyfullmember = " + Utils.ConvertToDBString(xiCollection["familyfullmember"], Utils.DataType.String) + ", ";
            query += "region = " + Utils.ConvertToDBString(xiCollection["region"], Utils.DataType.String) + ", ";
            query += "positioninregion = " + Utils.ConvertToDBString(xiCollection["positioninregion"], Utils.DataType.String) + ", ";
            query += "isbreeder = " + Utils.ConvertToDBString(xiCollection["isbreeder"], Utils.DataType.String) + ", ";
            query += "familymemberof = " + Utils.ConvertToDBString(xiCollection["familymemberof"], Utils.DataType.String) + ", ";
            query += "remark = " + Utils.ConvertToDBString(xiCollection["remark"], Utils.DataType.String) + ", ";
            query += "paymentmethod = " + Utils.ConvertToDBString(xiCollection["paymentmethod"], Utils.DataType.String) + ", ";
            query += "accountnumber = " + Utils.ConvertToDBString(xiCollection["accountnumber"], Utils.DataType.String) + ", ";
            query += "accountownername = " + Utils.ConvertToDBString(xiCollection["accountownername"], Utils.DataType.String) + ", ";
            query += "bankname = " + Utils.ConvertToDBString(xiCollection["bankname"], Utils.DataType.String) + ", ";
            query += "bankcode = " + Utils.ConvertToDBString(xiCollection["bankcode"], Utils.DataType.String) + ", ";
            query += "breedtype = " + Utils.ConvertToDBString(xiCollection["breedtype"], Utils.DataType.String) + ", ";
            query += "animalname = " + Utils.ConvertToDBString(xiCollection["animalname"], Utils.DataType.String) + ", ";
            query += "animalbirthdate = " + Utils.ConvertToDBString(xiCollection["animalbirthdate"], Utils.DataType.Date) + ", ";
            query += "collarid = " + Utils.ConvertToDBString(xiCollection["collarid"], Utils.DataType.String);
            query += " where id = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetMemberIdByEmailAddress(object xiEmailAddress, object xiAssociationId)
        {
            string query = @"select m.id from member m inner join [association_members] am on m.id = am.memberid where m.active = 1 and am.active = 1 
and m.email = " + Utils.ConvertToDBString(xiEmailAddress, Utils.DataType.String) + " and am.association_id = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer);


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object value = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return BusinessBase.ConvertToInteger(value);
        }

        public static int GetMemberListCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(*) maxcount from member m inner join [association_members] am on m.id = am.memberid where m.active = 1 and am.active = 1";
            if (!string.IsNullOrEmpty(xiFilter)) query += " and " + xiFilter;
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
                objdb.Write_log_file("GetMemberListCount", x.Message);
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

        public static DataSet GetMemberList(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select m.id,memberno,m.fname as fname,m.lname as lname,m.email as email,m.mobile as mobile,am.isinvitesend,ISNULL(am.isadmin,0) 'isadmin',m.exitdate from member m 
inner join [association_members] am on m.id = am.memberid where m.active = 1 and am.active = 1";
            if (!string.IsNullOrEmpty(xiFilter)) query += " and " + xiFilter;
            query += " order by m.fname,m.lname offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);

            objdb.Disconnectdb();
            return ds;
        }

        public static string SearchMember(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["associationid"] != null && xiCollection["associationid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(am.association_id = {0})", Utils.ConvertToDBString(xiCollection["associationid"], Utils.DataType.Integer)));
            }

            if (xiCollection["type"] != null && xiCollection["type"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;

                int type = BusinessBase.ConvertToInteger(xiCollection["type"]);
                if (type == 1)
                    builder.Append(string.Format("(m.exitdate is null )"));
                else if (type == 2)
                    builder.Append(string.Format("(m.exitdate is not null)"));
            }

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(m.fname + ' ' + m.lname like {0} or m.lname + ' ' + m.fname like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            return builder.ToString();
        }

        public int AddAssociationMembers(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            if (!xiCollection.AllKeys.Contains("active") && string.IsNullOrEmpty(xiCollection["active"])) xiCollection["active"] = "1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"insert into association_members([memberid],[association_id],[jointype],[active]) values(@memberid,@association_id,@jointype,@active)";

            Parameter param1 = new Parameter("memberid", xiCollection["memberid"], DbType.Int32);
            Parameter param2 = new Parameter("association_id", xiCollection["association_id"], DbType.Int32);
            Parameter param4 = new Parameter("jointype", 1, DbType.Int16);
            Parameter param5 = new Parameter("active", xiCollection["active"], DbType.Int16);

            string query2 = "select scope_identity()";

            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param4, param5 });
            objdb.Disconnectdb();

            if (value > 0)
                return value;
            else
                return int.MinValue;

        }

        public static bool UpdateInviteStatus(object xiMemberId, object xiAssociationId)
        {
            if (xiMemberId == null || xiAssociationId == null) return false;

            DBClass objdb = new DBClass();
            string query = @"update [association_members] set isinvitesend = 1 where memberid = " + Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer) + " and association_id = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer) + " ";

            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetAssociationJoinMembershipListCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(*) 'maxcount' from member m inner join [association_members] am on m.id = am.memberid where m.active = 2 and am.active = 2 and association_id = " + Utils.ConvertToDBString(xiFilter, Utils.DataType.Integer) + "";
            //if (!string.IsNullOrEmpty(xiFilter)) query += " and " + xiFilter;
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
                objdb.Write_log_file("GetAssociationMembershipListCount", x.Message);
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

        public static DataSet GetAssociationJoinMembershipList(int xiPage, string xiFilter, object xiUserid)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select m.id,fname,lname,m.email as 'email',m.submitdate,m.active from member m
inner join [association_members] am on m.id = am.memberid 
inner join animal_association aa on aa.id = am.association_id
where aa.active = 1 and am.active = 2 and m.active = 2 and am.association_id = " + Utils.ConvertToDBString(xiFilter, Utils.DataType.Integer) + " ";
            // and aa.createdby = " + Utils.ConvertToDBString(xiUserid, Utils.DataType.Integer) + " #todo comment26dept

            query += " order by m.id desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_submitdate", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                        if (tempDate1 != DateTime.MinValue)
                        {
                            row["procesed_submitdate"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                        }
                    }
                    catch
                    { }

                }
            }

            return ds;
        }

        public static bool UpdateApproveMembership(object xiMemberId, string xiAssociationId)
        {
            NameValueCollection mCollection = Member.GetUnApproveMember(xiMemberId);
            if (mCollection == null) return false;

            string query = "update member set active = 1  where id = " + Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer) + " and active = 2; update [association_members] set active = 1  where memberid = " + Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer) + " and association_id = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer) + " and active = 2;";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool UpdateRejectMembership(object xiMemberId, string xiAssociationId)
        {
            NameValueCollection mCollection = Member.GetUnApproveMember(xiMemberId);
            if (mCollection == null || string.IsNullOrEmpty(xiAssociationId)) return false;

            string query = @"Delete [association_members] where active = 2 and memberid = {0} and  association_id = {1};  Delete member where active = 2 and id = {0};";
            query = string.Format(query, Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer), Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetUnApproveMember(object xiMemberId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from member where active = 2 and id = " + Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("memberno", Convert.ToString(objdb.GetValue(reader1, "memberno")));
                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("street", Convert.ToString(objdb.GetValue(reader1, "street")));
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    collection.Add("city", Convert.ToString(objdb.GetValue(reader1, "city")));
                    collection.Add("country", Convert.ToString(objdb.GetValue(reader1, "country")));
                    collection.Add("zipcode", Convert.ToString(objdb.GetValue(reader1, "zipcode")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("mobile", Convert.ToString(objdb.GetValue(reader1, "mobile")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("fax", Convert.ToString(objdb.GetValue(reader1, "fax")));
                    collection.Add("entrydate", Convert.ToString(objdb.GetValue(reader1, "entrydate")));
                    collection.Add("exitdate", Convert.ToString(objdb.GetValue(reader1, "exitdate")));
                    collection.Add("exitreason", Convert.ToString(objdb.GetValue(reader1, "exitreason")));
                    collection.Add("membershiptype", Convert.ToString(objdb.GetValue(reader1, "membershiptype")));
                    collection.Add("familyfullmember", Convert.ToString(objdb.GetValue(reader1, "familyfullmember")));
                    collection.Add("region", Convert.ToString(objdb.GetValue(reader1, "region")));
                    collection.Add("positioninregion", Convert.ToString(objdb.GetValue(reader1, "positioninregion")));
                    collection.Add("isbreeder", Convert.ToString(objdb.GetValue(reader1, "isbreeder")));
                    collection.Add("familymemberof", Convert.ToString(objdb.GetValue(reader1, "familymemberof")));
                    collection.Add("remark", Convert.ToString(objdb.GetValue(reader1, "remark")));
                    collection.Add("paymentmethod", Convert.ToString(objdb.GetValue(reader1, "paymentmethod")));
                    collection.Add("accountnumber", Convert.ToString(objdb.GetValue(reader1, "accountnumber")));
                    collection.Add("accountownername", Convert.ToString(objdb.GetValue(reader1, "accountownername")));
                    collection.Add("bankname", Convert.ToString(objdb.GetValue(reader1, "bankname")));
                    collection.Add("bankcode", Convert.ToString(objdb.GetValue(reader1, "bankcode")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("animalname", Convert.ToString(objdb.GetValue(reader1, "animalname")));
                    collection.Add("animalbirthdate", Convert.ToString(objdb.GetValue(reader1, "animalbirthdate")));
                    collection.Add("collarid", Convert.ToString(objdb.GetValue(reader1, "collarid")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("submitby", Convert.ToString(objdb.GetValue(reader1, "submitby")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetMember", x.Message);
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

        public bool ExitMemeber(NameValueCollection xiCollection, object xiEmailAddress, object xiAssociationId)
        {
            if (xiCollection == null || xiEmailAddress == null || xiAssociationId == null) return false;

            int memberId = Member.GetMemberIdByEmailAddress(xiEmailAddress, xiAssociationId);
            if (memberId <= 0) return false;
            bool chkStatus = UpdateISAdminStatus(0, memberId, xiAssociationId);

            string query = @"UPDATE [member] SET exitdate = @exitdate,exitreason = @exitreason WHERE id = " + Utils.ConvertToDBString(memberId, Utils.DataType.Integer);

            Parameter param1 = new Parameter("exitdate", xiCollection["exitdate"], DbType.Date);
            Parameter param2 = new Parameter("exitreason", xiCollection["exitreason"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool UpdateAdmin(object xiMemberId, object xiAssociationId)
        {
            string query = @"update [association_members] set isinvitesend = 1 where memberid = " + Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer) + " and association_id = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer) + " ";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool UpdateISAdminStatus(object xiType, object xiMemberId, object xiAssociationId)
        {
            int typeVal = BusinessBase.ConvertToInteger(xiType);

            string query = string.Empty;
            if (typeVal == 0)
                query = @"update [association_members] set isadmin = 0 where memberid = {0} and association_id = {1} and active = 1"; // exit as admin
            else if (typeVal == 1)
                query = @"update [association_members] set isadmin = 1 where memberid = {0} and association_id = {1} and active = 1"; // add as admin

            query = string.Format(query, Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer), Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int CheckISAdminStatus(object xiEmail, object xiAssociationId)
        {
            string query = @"select ISNULL(isadmin,0) as 'isadmin' from [association_members] where memberid = (select m.id from [member] m  where m.active = 1 and m.exitdate is null and m.email = " + Utils.ConvertToDBString(xiEmail, Utils.DataType.String) + " ) and association_id = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer) + " and active = 1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object value = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return (BusinessBase.ConvertToInteger(value) > 0) ? 1 : 0;
        }

        public static int CheckIsAssociationMemberEmailExist(object xiEmail, object xiAssociationId, object xiMemberId)
        {
            string query = @"select count(*) as 'emailcount' from association_members am inner join [member] m on am.memberid = m.id and am.active = 1 where m.active = 1 and m.email = " + Utils.ConvertToDBString(xiEmail, Utils.DataType.String) + " and am.association_id = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer);
            if (xiMemberId != null && xiMemberId.ToString().Length > 0) query += "and m.id <> " + Utils.ConvertToDBString(xiMemberId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object value = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return (BusinessBase.ConvertToInteger(value) > 0) ? 1 : 0;
        }


        #endregion
    }
}
