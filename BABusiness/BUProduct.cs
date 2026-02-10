using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BABusiness
{
    public class BUProduct : BusinessBase
    {
        public enum Status
        {
            PRODUCTADD = 1,
            PRODUCTEDIT = 2,
            PRODUCTDELETE = 3,
            PRODUCTIMAGEADD = 4,
            PRODUCTIMAGEDELETE = 5,
        }

        public static string GetStatusLogText(object xiStatus, object xiAdditionMessage)
        {
            string statusText = string.Empty;
            try
            {
                switch (Convert.ToInt32(xiStatus))
                {
                    case (int)Status.PRODUCTADD:
                        statusText = "The product has been added.";
                        break;

                    case (int)Status.PRODUCTEDIT:
                        statusText = "The product has been edited.";
                        break;

                    case (int)Status.PRODUCTDELETE:
                        statusText = "The product has been deleted.";
                        break;

                    case (int)Status.PRODUCTIMAGEADD:
                        statusText = "The product image has been added.";
                        break;

                    case (int)Status.PRODUCTIMAGEDELETE:
                        statusText = "The product image has been deleted.";
                        break;

                    default:
                        statusText = "-";
                        break;
                }
            }
            catch { }

            return statusText;
        }

        #region Product

        public static int AddProduct(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;
            string query = "INSERT INTO [bu_product]([bu_id],[category_id],[brand_id],[material_type],[name],[description],[product_code],[cost],[final_cost],[status],[size],[color],[weight],[thresholdstockvalue],[created],[createdby],[updated],[updatedby],[profileimage],[active],[taxid]) values(@bu_id,@category_id,@brand_id,@material_type,@name,@description,@product_code,@cost,@final_cost,@status,@size,@color,@weight,@thresholdstockvalue,getutcdate(),@createdby,getutcdate(),@updatedby,@profileimage,@active,@taxid)";
            Parameter param1 = new Parameter("bu_id", xiCollection["companyid"], DbType.Int32);
            Parameter param2 = new Parameter("category_id", xiCollection["category_id"], DbType.Int32);
            Parameter param3 = new Parameter("brand_id", xiCollection["brand_id"], DbType.Int32);
            Parameter param4 = new Parameter("material_type", xiCollection["material_type"]);
            Parameter param5 = new Parameter("name", xiCollection["name"]);
            Parameter param6 = new Parameter("description", xiCollection["description"]);
            Parameter param7 = new Parameter("product_code", xiCollection["product_code"]);
            Parameter param8 = new Parameter("cost", xiCollection["cost"], DbType.Int32);
            Parameter param9 = new Parameter("final_cost", xiCollection["final_cost"], DbType.Int32);
            Parameter param10 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param11 = new Parameter("size", xiCollection["size"]);
            Parameter param12 = new Parameter("color", xiCollection["color"]);
            Parameter param13 = new Parameter("weight", xiCollection["weight"]);
            Parameter param14 = new Parameter("thresholdstockvalue", xiCollection["thresholdstockvalue"], DbType.Int32);
            //Parameter param14 = new Parameter("original_quantity", xiCollection["original_quantity"], DbType.Int32);
            //Parameter param15 = new Parameter("stock_quantity", xiCollection["stock_quantity"], DbType.Int32);
            Parameter param16 = new Parameter("createdby", xiCollection["userid"], DbType.Int32);
            Parameter param17 = new Parameter("updatedby", xiCollection["userid"], DbType.Int32);
            Parameter param18 = new Parameter("profileimage", xiCollection["profileimage"]);
            Parameter param19 = new Parameter("active", "1");
            Parameter param20 = new Parameter("taxid", xiCollection["taxid"], DbType.Int32);


            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param16, param17, param18, param19, param20 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateProduct(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [bu_product] set bu_id=@bu_id,category_id=@category_id,brand_id=@brand_id,material_type=@material_type,name=@name,description=@description,cost=@cost,final_cost=@final_cost,status=@status,size=@size,color=@color,weight=@weight,thresholdstockvalue=@thresholdstockvalue,updated=getutcdate(),updatedby=@updatedby,profileimage=@profileimage,taxid=@taxid where id=@id";
            Parameter param1 = new Parameter("bu_id", xiCollection["companyid"], DbType.Int32);
            Parameter param2 = new Parameter("category_id", xiCollection["category_id"], DbType.Int32);
            Parameter param3 = new Parameter("brand_id", xiCollection["brand_id"], DbType.Int32);
            Parameter param4 = new Parameter("material_type", xiCollection["material_type"]);
            Parameter param5 = new Parameter("name", xiCollection["name"]);
            Parameter param6 = new Parameter("description", xiCollection["description"]);
            Parameter param7 = new Parameter("cost", xiCollection["cost"], DbType.Int32);
            Parameter param8 = new Parameter("final_cost", xiCollection["final_cost"], DbType.Int32);
            Parameter param9 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param10 = new Parameter("size", xiCollection["size"]);
            Parameter param11 = new Parameter("color", xiCollection["color"]);
            Parameter param12 = new Parameter("weight", xiCollection["weight"]);
            //Parameter param13 = new Parameter("original_quantity", xiCollection["original_quantity"], DbType.Int32);
            //Parameter param14 = new Parameter("stock_quantity", xiCollection["stock_quantity"], DbType.Int32);
            Parameter param13 = new Parameter("thresholdstockvalue", xiCollection["thresholdstockvalue"], DbType.Int32);
            Parameter param15 = new Parameter("updatedby", xiCollection["userid"], DbType.Int32);
            Parameter param16 = new Parameter("profileimage", xiCollection["profileimage"]);
            Parameter param17 = new Parameter("taxid", xiCollection["taxid"], DbType.Int32);
            Parameter param18 = new Parameter("id", xiId, DbType.Int32);


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param15, param16, param17, param18 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string DeleteProduct(object xiId, string xiUserId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string query = @"select count(a.id) as maxcount from bu_product_images a 
   inner join bu_product c on a.productid = c.id where a.productid = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("Deleteproduct", x.Message);
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
                query = string.Format("update bu_product set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

                objdb.Connectdb();
                int value = objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();

                returnValue = (value > 0) ? "Deleted Successfully." : "false";
                //log for delete
                NameValueCollection collection = new NameValueCollection();
                collection["product_id"] = xiId.ToString();
                collection["user_id"] = xiUserId.ToString();
                collection["message_id"] = (int)BUProduct.Status.PRODUCTDELETE + "";
                collection["old_entry"] = string.Empty;
                collection["new_entry"] = string.Empty;
                collection["comment"] = "Product deleted.";
                BUProduct.AddProductLog(collection);
            }
            else
            {
                returnValue = "You can not delete this item because its already used.";
            }
            objdb.Disconnectdb();

            return returnValue;
        }

        public static int GetProductCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from [bu_product] c inner join bu_tax t on t.id= c.taxid inner join view_bu_product_stock vps on vps.id= c.id where c.active = 1";
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
                objdb.Write_log_file("GetProductCount", x.Message);
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

        public static DataSet GetProductDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select c.*,t.[name] as taxname,t.[percentage] as taxpercentage,vps.totalstock,vps.usedstock,vps.availablestock from [bu_product] c inner join bu_tax t on t.id= c.taxid inner join view_bu_product_stock vps on vps.id= c.id where c.active = 1";
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


        public static int GetProductOutOfStockCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from [bu_product] c inner join bu_tax t on t.id= c.taxid inner join view_bu_product_stock vps on vps.id= c.id where c.active = 1 and vps.availablestock <= c.thresholdstockvalue";
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
                objdb.Write_log_file("GetProductOutOfStockCount", x.Message);
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

        public static DataSet GetProductOutOfStockDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select c.*,t.[name] as taxname,t.[percentage] as taxpercentage,vps.totalstock,vps.usedstock,vps.availablestock from [bu_product] c inner join bu_tax t on t.id= c.taxid inner join view_bu_product_stock vps on vps.id= c.id where c.active = 1 and vps.availablestock <= c.thresholdstockvalue";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.name offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }        

        public static NameValueCollection GetProductDetail(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;

            string query = "select c.*,b.[name] as brandname,a.[name] as categoryname,t.[name] as taxname,t.[percentage] as taxpercentage,vps.totalstock,vps.usedstock,vps.availablestock from bu_product c inner join bu_product_category a on c.category_id = a.id  inner join bu_product_brand b on c.brand_id = b.id inner join bu_tax t on t.id= c.taxid inner join view_bu_product_stock vps on vps.id= c.id where c.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and c.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and c.active = 1";
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
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("product_code", Convert.ToString(objdb.GetValue(reader1, "product_code")));
                    //  collection.Add("type_id", Convert.ToString(objdb.GetValue(reader1, "type_id")));
                    collection.Add("cost", Convert.ToString(objdb.GetValue(reader1, "cost")));
                    collection.Add("final_cost", Convert.ToString(objdb.GetValue(reader1, "final_cost")));
                    collection.Add("description", Convert.ToString(objdb.GetValue(reader1, "description")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                    collection.Add("updatedby", Convert.ToString(objdb.GetValue(reader1, "updatedby")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("category_id", Convert.ToString(objdb.GetValue(reader1, "category_id")));
                    collection.Add("brand_id", Convert.ToString(objdb.GetValue(reader1, "brand_id")));
                    collection.Add("material_type", Convert.ToString(objdb.GetValue(reader1, "material_type")));
                    collection.Add("size", Convert.ToString(objdb.GetValue(reader1, "size")));
                    collection.Add("color", Convert.ToString(objdb.GetValue(reader1, "color")));
                    collection.Add("weight", Convert.ToString(objdb.GetValue(reader1, "weight")));
                    //collection.Add("original_quantity", Convert.ToString(objdb.GetValue(reader1, "original_quantity")));
                    //collection.Add("stock_quantity", Convert.ToString(objdb.GetValue(reader1, "stock_quantity")));
                    collection.Add("thresholdstockvalue", Convert.ToString(objdb.GetValue(reader1, "thresholdstockvalue")));
                    collection.Add("categoryname", Convert.ToString(objdb.GetValue(reader1, "categoryname")));
                    collection.Add("brandname", Convert.ToString(objdb.GetValue(reader1, "brandname")));
                    collection.Add("profileimage", Convert.ToString(objdb.GetValue(reader1, "profileimage")));
                    collection.Add("taxid", Convert.ToString(objdb.GetValue(reader1, "taxid")));
                    collection.Add("taxname", Convert.ToString(objdb.GetValue(reader1, "taxname")));
                    collection.Add("taxpercentage", Convert.ToString(objdb.GetValue(reader1, "taxpercentage")));

                    collection.Add("totalstock", Convert.ToString(objdb.GetValue(reader1, "totalstock")));
                    collection.Add("usedstock", Convert.ToString(objdb.GetValue(reader1, "usedstock")));
                    collection.Add("availablestock", Convert.ToString(objdb.GetValue(reader1, "availablestock")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetProductDetail", x.Message);
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

            if (xiCollection["brand_id"] != null && xiCollection["brand_id"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.brand_id = {0})", Utils.ConvertToDBString(xiCollection["brand_id"], Utils.DataType.Integer)));
            }

            if (xiCollection["category_id"] != null && xiCollection["category_id"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.category_id = {0})", Utils.ConvertToDBString(xiCollection["category_id"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static bool DeleteTags(object xiId)
        {
            DBClass objdb = new DBClass();
            string query = string.Format("delete from bu_product_tags where product_id=@product_id");
            Parameter param1 = new Parameter("product_id", xiId, DbType.Int32);

            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int AddProductTag(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [bu_product_tags]([name],[product_id]) values(@name,@product_id)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("product_id", xiCollection["product_id"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public static DataTable GetProductTagDetails(object xiProductId)
        {
            string query = "select c.* from bu_product_tags c where c.product_id=" + Utils.ConvertToDBString(xiProductId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static bool AddProductLog(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into bu_product_log(product_id, user_id, [datetime],message_id, old_entry, new_entry, comment) values(@product_id,@user_id,getutcdate(),@message_id,@old_entry,@new_entry,@comment)";

            Parameter param1 = new Parameter("product_id", xiCollection["product_id"], DbType.Int32);
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

        #region Product_Gallery

        public static bool AddProductGallery(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [bu_product_images]([productid],[title],[gallery_file],[file_type],[active],[submitdate]) values(@productid,@title,@file_name,@file_type,1,getutcdate())";
            Parameter param1 = new Parameter("productid", xiCollection["productid"], DbType.Int32);
            Parameter param2 = new Parameter("file_name", xiCollection["file_name"]);
            Parameter param3 = new Parameter("title", xiCollection["title"]);
            Parameter param4 = new Parameter("file_type", xiCollection["file_type"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["product_id"] = xiCollection["productid"];
            logCollection["user_id"] = xiCollection["userid"];
            logCollection["message_id"] = (int)BUProduct.Status.PRODUCTIMAGEADD + "";
            logCollection["old_entry"] = string.Empty;
            logCollection["new_entry"] = string.Empty;
            logCollection["comment"] = xiCollection["file_name"];
            BUProduct.AddProductLog(logCollection);

            return (value > 0);
        }

        public static DataSet GetProductPhotos(object xiProductId)
        {
            string query = "select * from bu_product_images where active=1 and productid=" + Utils.ConvertToDBString(xiProductId, Utils.DataType.Integer) + " order by submitdate desc";

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

            string query = "select * from bu_product_images  where active=1 and productid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

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

        public static bool DeleteProductGalleryPhoto(string xiFileName, NameValueCollection xiCollection)
        {
            string query = "delete from [bu_product_images] where [gallery_file]=@file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();
            if (xiCollection != null)
            {
                NameValueCollection logCollection = new NameValueCollection();
                logCollection["product_id"] = xiCollection["productid"];
                logCollection["user_id"] = xiCollection["userid"];
                logCollection["message_id"] = (int)BUProduct.Status.PRODUCTIMAGEDELETE + "";
                logCollection["old_entry"] = string.Empty;
                logCollection["new_entry"] = string.Empty;
                logCollection["comment"] = xiFileName;
                BUProduct.AddProductLog(logCollection);
            }
            return (value > 0);

        }

        #endregion

        #region Product Category 

        public static int AddProductCategory(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "insert into bu_product_category(name,bu_id,active)values(@name,@bu_id,1)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("bu_id", xiCollection["companyid"]);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateProductCategory(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_product_category set name=@name where id=@id and active = 1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetProductCategory(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_product_category where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

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
                objdb.Write_log_file("GetProductCategory", x.Message);
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

        public static NameValueCollection GetProductTypeClassName(object xiName)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_product_category where active=1 and name=" + Utils.ConvertToDBString(xiName, Utils.DataType.String);

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
                objdb.Write_log_file("GetProductTypeClassName", x.Message);
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

        public static DataSet GetProductCategoryDetails(string xiFilter, string xiBUId)
        {
            string query = "select ct.id,ct.[name] as 'typename',ct.bu_id from bu_product_category ct where ct.active = 1 and ct.bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by ct.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataTable GetProductCategory(string xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_product_category where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetProductCategoryTypes(string xiCategoryId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_product_category where id = " + Utils.ConvertToDBString(xiCategoryId, Utils.DataType.Integer) + " and active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string DeleteProductCategory(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from bu_product where category_id = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1 ";
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
                objdb.Write_log_file("DeleteProductCategory", x.Message);
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
                query = string.Format("update bu_product_category set active = 0 where id= {0}", xiId);
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

        public static int ProductCategoryCount(object xiBUId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(id) as maxcount from bu_product_category where bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("ProductCategoryCount", x.Message);
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

            return totalCount;
        }

        #endregion

        #region Product Brand

        public static int AddProductBrand(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "insert into bu_product_brand(name,bu_id,active)values(@name,@bu_id,1)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("bu_id", xiCollection["companyid"]);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateProductBrand(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_product_brand set name=@name where id=@id and active = 1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetProductBrands(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_product_brand where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

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
                objdb.Write_log_file("GetProductBrands", x.Message);
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

        public static NameValueCollection GetProductBrandClassName(object xiName)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_product_brand where active=1 and name=" + Utils.ConvertToDBString(xiName, Utils.DataType.String);

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
                objdb.Write_log_file("GetProductBrandClassName", x.Message);
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

        public static DataSet GetProductBrandDetails(string xiFilter, string xiBUId)
        {
            string query = "select ct.id,ct.[name] as 'typename',ct.bu_id from bu_product_brand ct where ct.active = 1 and ct.bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by ct.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataTable GetProductBrand(string xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_product_brand where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetProductBrandTypes(string xiBrandId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_product_brand where id = " + Utils.ConvertToDBString(xiBrandId, Utils.DataType.Integer) + " and active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string DeleteProductBrand(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from bu_product where brand_id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteProductBrand", x.Message);
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
                query = string.Format("update bu_product_brand set active = 0 where id= {0}", xiId);
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

        public static int ProductBrandCount(object xiBUId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(id) as maxcount from bu_product_brand where bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("ProductBrandCount", x.Message);
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

            return totalCount;
        }

        #endregion

        #region Combo

        public static int AddCombo(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;
            string query = "INSERT INTO [bu_combodetails]([bu_id],[title],[productlist],[servicelist],[cost],[profileimage],[createdby],[createddate],[taxid]) values(@bu_id,@title,@productlist,@servicelist,@cost,@profileimage,@createdby,getutcdate(),@taxid)";
            Parameter param1 = new Parameter("bu_id", xiCollection["companyid"], DbType.Int32);
            Parameter param2 = new Parameter("title", xiCollection["title"]);
            Parameter param3 = new Parameter("productlist", xiCollection["productlist"]);
            Parameter param4 = new Parameter("servicelist", xiCollection["servicelist"]);
            Parameter param5 = new Parameter("cost", xiCollection["cost"], DbType.Int32);
            Parameter param6 = new Parameter("profileimage", xiCollection["profileimage"]);
            Parameter param7 = new Parameter("createdby", xiCollection["createdby"], DbType.Int32);
            Parameter param8 = new Parameter("taxid", xiCollection["taxid"], DbType.Int32);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateCombo(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [bu_combodetails] set title=@title,productlist=@productlist,servicelist=@servicelist,cost=@cost,profileimage=@profileimage,updatedby=@updatedby,updateddate=getutcdate(),taxid=@taxid where id=@id";

            Parameter param1 = new Parameter("title", xiCollection["title"]);
            Parameter param2 = new Parameter("productlist", xiCollection["productlist"]);
            Parameter param3 = new Parameter("servicelist", xiCollection["servicelist"]);
            Parameter param4 = new Parameter("cost", xiCollection["cost"], DbType.Int32);
            Parameter param5 = new Parameter("profileimage", xiCollection["profileimage"]);
            Parameter param6 = new Parameter("updatedby", xiCollection["updatedby"], DbType.Int32);
            Parameter param7 = new Parameter("taxid", xiCollection["taxid"], DbType.Int32);
            Parameter param8 = new Parameter("id", xiId, DbType.Int32);


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetComboDetail(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;

            string query = "select cd.*,t.[name] as taxname,t.[percentage] as taxpercentage from bu_combodetails cd inner join bu_tax t on t.id= cd.taxid where cd.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and cd.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and cd.active = 1";
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
                    collection.Add("title", Convert.ToString(objdb.GetValue(reader1, "title")));
                    collection.Add("productlist", Convert.ToString(objdb.GetValue(reader1, "productlist")));
                    collection.Add("servicelist", Convert.ToString(objdb.GetValue(reader1, "servicelist")));
                    collection.Add("cost", Convert.ToString(objdb.GetValue(reader1, "cost")));
                    collection.Add("profileimage", Convert.ToString(objdb.GetValue(reader1, "profileimage")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("createddate", Convert.ToString(objdb.GetValue(reader1, "createddate")));
                    collection.Add("updatedby", Convert.ToString(objdb.GetValue(reader1, "updatedby")));
                    collection.Add("updateddate", Convert.ToString(objdb.GetValue(reader1, "updateddate")));
                    collection.Add("taxid", Convert.ToString(objdb.GetValue(reader1, "taxid")));
                    collection.Add("taxname", Convert.ToString(objdb.GetValue(reader1, "taxname")));
                    collection.Add("taxpercentage", Convert.ToString(objdb.GetValue(reader1, "taxpercentage")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetComboDetail", x.Message);
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

        public static int GetComboCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(cd.id) as maxcount from [bu_combodetails] cd inner join bu_tax t on t.id= cd.taxid where cd.active = 1";
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
                objdb.Write_log_file("GetComboCount", x.Message);
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

        public static DataSet GetComboDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select cd.*,(select stuff((select ', ' + p.[name] from bu_product p where p.active = 1 and p.id in (select * from dbo.STRING_SPLIT_2(cd.productlist, ';')) for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as productnames,(select stuff((select ', ' + s.[name] from bu_services s where s.active = 1 and s.id in (select * from dbo.STRING_SPLIT_2(cd.servicelist, ';')) for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as servicenames,t.[name] as taxname,t.[percentage] as taxpercentage from [bu_combodetails] cd inner join bu_tax t on t.id= cd.taxid where cd.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by cd.title offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
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

        public static string SearchCombo(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(cd.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }


            if (xiCollection["title"] != null && xiCollection["title"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(cd.title like {0})", Utils.ConvertToDBString("%" + xiCollection["title"] + "%", Utils.DataType.String)));
            }
            return builder.ToString();
        }

        public static bool DeleteCombo(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_combodetails set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetProductForComboCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from [bu_product] c inner join bu_tax t on t.id= c.taxid  where c.active = 1";
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
                objdb.Write_log_file("GetProductForComboCount", x.Message);
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

        public static DataSet GetProductForComboDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select c.*,t.[name] as taxname,t.[percentage] as taxpercentage from [bu_product] c inner join bu_tax t on t.id= c.taxid where c.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.name offset " + (xiPage * 10) + " rows fetch next " + 10 + " rows only";
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

        #endregion

        #region BU Tax

        public static int AddBUTax(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "insert into bu_tax(name,percentage,bu_id)values(@name,@percentage,@bu_id)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("percentage", xiCollection["percentage"],DbType.Int32);
            Parameter param3 = new Parameter("bu_id", xiCollection["companyid"]);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateBUTax(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_tax set name=@name,percentage=@percentage where id=@id and active = 1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("percentage", xiCollection["percentage"], DbType.Int32);
            Parameter param3 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetBUTax(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,percentage,bu_id from bu_tax where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("percentage", Convert.ToString(objdb.GetValue(reader1, "percentage")));
                    collection.Add("bu_id", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBUTax", x.Message);
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
      
        public static DataSet GetBUTaxDetails(string xiFilter, string xiBUId)
        {
            string query = "select t.id,t.[name] as 'taxname',t.percentage,t.bu_id from bu_tax t where t.active = 1 and t.bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by t.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataTable GetAllBUTax(string xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_tax where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetTax(string xiTaxId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_tax where id = " + Utils.ConvertToDBString(xiTaxId, Utils.DataType.Integer) + " and active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string DeleteBUTax(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from bu_businessuser where taxid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteBUTax", x.Message);
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
                query = string.Format("update bu_tax set active = 0 where id= {0}", xiId);
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


        #endregion

        #region BU Currency

        public static int AddBUCurrency(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "insert into bu_currency(name,bu_id)values(@name,@bu_id)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("bu_id", xiCollection["companyid"]);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateBUCurrency(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_currency set name=@name where id=@id and active = 1";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetBUCurrency(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_currency where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

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
                objdb.Write_log_file("GetBUCurrency", x.Message);
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

        public static DataSet GetBUCurrencyDetails(string xiFilter, string xiBUId)
        {
            string query = "select c.id,c.[name] as 'currencyname',c.bu_id from bu_currency c where c.active = 1 and c.bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataTable GetAllBUCurency(string xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_currency where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetCurrency(string xiTaxId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_currency where id = " + Utils.ConvertToDBString(xiTaxId, Utils.DataType.Integer) + " and active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string DeleteBUCurrency(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from bu_businessuser where currencyid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteBUCurrency", x.Message);
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
                query = string.Format("update bu_currency set active = 0 where id= {0}", xiId);
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


        #endregion

        #region Product Stock

        public static int AddProductStock(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;
            string query = "INSERT INTO [bu_product_stock]([productid],[stockquan],[date],[ponumber],[staffid]) values(@productid,@stockquan,@date,@ponumber,@staffid)";
            Parameter param1 = new Parameter("productid", xiCollection["productid"], DbType.Int32);
            Parameter param2 = new Parameter("stockquan", xiCollection["stockquan"], DbType.Int32);
            Parameter param3 = new Parameter("date", xiCollection["date"], DbType.DateTime);
            Parameter param4 = new Parameter("ponumber", xiCollection["ponumber"]);
            Parameter param5 = new Parameter("staffid ", xiCollection["staffid"], DbType.Int32);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5 });
            objdb.Disconnectdb();

            return value;
        }


        public static bool UpdateProductStock(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_product_stock set stockquan=@stockquan,date=@date,ponumber=@ponumber where id=@id";

            Parameter param1 = new Parameter("stockquan", xiCollection["stockquan"], DbType.Int32);
            Parameter param2 = new Parameter("date", xiCollection["date"], DbType.DateTime);
            Parameter param3 = new Parameter("ponumber", xiCollection["ponumber"]);
            Parameter param4 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3,param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataSet GetAllProductStock(int xiPage, string xiProductId)
        {
            if (string.IsNullOrEmpty(xiProductId)) return null;

            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select ps.*, u.fname as user_pre_name, u.lname as user_family_name from bu_product_stock ps inner join [user] u on u.id = ps.staffid where ps.productid = " + Utils.ConvertToDBString(xiProductId, Utils.DataType.Integer) + " and ps.active=1 order by ps.date desc";

            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("addedby", typeof(string));
                ds.Tables[0].Columns.Add("formatteddate", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["addedby"] = row["user_pre_name"] + " " + row["user_family_name"];

                    DateTime tempDate = Convert.ToDateTime(row["date"]);
                    if (tempDate != DateTime.MinValue)
                    {
                        row["formatteddate"] = tempDate.ToString(BusinessBase.DateFormat);
                    }
                }
            }

            return ds;
        }

        public static NameValueCollection GetProductStock(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,productid,date,stockquan,ponumber,staffid from bu_product_stock where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("productid", Convert.ToString(objdb.GetValue(reader1, "productid")));
                    collection.Add("date", Convert.ToString(objdb.GetValue(reader1, "date")));
                    collection.Add("stockquan", Convert.ToString(objdb.GetValue(reader1, "stockquan")));
                    collection.Add("ponumber", Convert.ToString(objdb.GetValue(reader1, "ponumber")));
                    collection.Add("staffid", Convert.ToString(objdb.GetValue(reader1, "staffid")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetProductStock", x.Message);
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

        public static bool DeleteProductStock(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_product_stock set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        #endregion
    }
}
