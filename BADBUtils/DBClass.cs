using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace BADBUtils
{
    public class DBClass
    {
        public DBClass()
        {
        }
                
        private static string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        public static string ApplicationBasePath = System.Configuration.ConfigurationManager.AppSettings["basepath"];

        public SqlConnection con;
        private SqlCommand cmd;
        private SqlDataReader Dr;
        private SqlDataAdapter Da;
        private DataSet Ds;
        private Object obj;

        public int ExecuteNonQuery(SqlConnection oCon, string sSql)
        {
            if (oCon == null || oCon.State != ConnectionState.Open) return int.MinValue;

            int iRet = int.MinValue;
            try
            {
                cmd = new SqlCommand(sSql, oCon);
                iRet = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception x) { this.Write_log_file("ExecuteNonQuery", x.Message); }

            return iRet;
        }

        public int ExecuteNonQuery(SqlConnection oCon, string sSql, Parameter[] paramList)
        {
            if (oCon == null || oCon.State != ConnectionState.Open) return int.MinValue;

            int iRet = int.MinValue;
            try
            {
                cmd = new SqlCommand(sSql, oCon);
                if (paramList != null)
                {
                    foreach (Parameter param in paramList)
                    {
                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.ParameterName = "@" + param.ParameterName;
                        sqlParam.Value = param.ParameterValue;
                        sqlParam.DbType = param.Type;
                        cmd.Parameters.Add(sqlParam);
                    }
                }
                iRet = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception x) { this.Write_log_file("ExecuteNonQuery2", x.Message); }

            return iRet;
        }

        public int ExecuteNonQueryScopeIdentity(SqlConnection oCon, string sSql1, string sSql2)
        {
            if (oCon == null || oCon.State != ConnectionState.Open) return int.MinValue;

            int iRet = int.MinValue;
            try
            {
                cmd = new SqlCommand(sSql1, oCon);
                cmd.ExecuteNonQuery();
                cmd.CommandText = sSql2;
                object obj = cmd.ExecuteScalar();
                iRet = Convert.ToInt32(obj);
                cmd.Dispose();
            }
            catch (Exception x) { this.Write_log_file("ExecuteNonQueryScopeIdentity", x.Message); }

            return iRet;


        }

        public int ExecuteNonQueryScopeIdentity(SqlConnection oCon, string sSql1, string sSql2, Parameter[] paramList)
        {
            if (oCon == null || oCon.State != ConnectionState.Open) return int.MinValue;

            object iRet = DBNull.Value;
            try
            {
                cmd = new SqlCommand(sSql1 + ";" + sSql2, oCon);
                if (paramList != null)
                {
                    foreach (Parameter param in paramList)
                    {
                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.ParameterName = "@" + param.ParameterName;
                        sqlParam.Value = param.ParameterValue;
                        sqlParam.DbType = param.Type;
                        cmd.Parameters.Add(sqlParam);
                    }
                }
                iRet = cmd.ExecuteScalar();
                cmd.Dispose();

                if (iRet == DBNull.Value) return int.MinValue;

                return Convert.ToInt32(iRet);
            }
            catch (Exception x) { this.Write_log_file("ExecuteNonQueryScopeIdentity", x.Message); }

            return int.MinValue;
        }

        public SqlDataReader ExecuteReader(SqlConnection oCon, string sSql)
        {
            return ExecuteReader(oCon, sSql, null);
        }

        public SqlDataReader ExecuteReader(SqlConnection oCon, string sSql, Parameter[] paramList)
        {
            if (oCon == null || oCon.State != ConnectionState.Open) return null;

            try
            {
                sSql = Utils.ColumnToDecrypt(sSql);
                cmd = new SqlCommand(sSql, oCon);

                if (paramList != null)
                {
                    foreach (Parameter param in paramList)
                    {
                        SqlParameter parameter = new SqlParameter();
                        parameter.ParameterName = "@" + param.ParameterName;
                        parameter.Value = param.ParameterValue;
                        parameter.DbType = param.Type;
                        parameter.Direction = param.Direction;
                        cmd.Parameters.Add(parameter);
                    }
                }

                Dr = cmd.ExecuteReader();
                cmd.Dispose();
            }
            catch (Exception x) { this.Write_log_file("ExecuteReader", x.Message); }

            return Dr;
        }

        public Object ExecuteScalar(SqlConnection oCon, string sSql)
        {
            if (oCon == null || oCon.State != ConnectionState.Open) return null;

            try
            {
                sSql = Utils.ColumnToDecrypt(sSql);
                cmd = new SqlCommand(sSql, oCon);
                obj = cmd.ExecuteScalar();
                cmd.Dispose();
            }
            catch (Exception x) { this.Write_log_file("ExecuteScalar", x.Message); }

            return obj;
        }

        public DataSet ExecuteDataSet(SqlConnection oCon, string sSql)
        {
            if (oCon == null || oCon.State != ConnectionState.Open) return null;

            try
            {
                sSql = Utils.ColumnToDecrypt(sSql);
                Da = new SqlDataAdapter(sSql, oCon);
                Ds = new DataSet();
                Da.Fill(Ds);
                Da.Dispose();

                if (Ds != null && Ds.Tables.Count > 0)
                {
                    foreach (DataTable Dt in Ds.Tables)
                    {
                        foreach (DataColumn dcol in Dt.Columns)
                        {
                            foreach (DataRow dtRow in Dt.Rows)
                            {
                                if (dcol.DataType == typeof(DateTime) || dcol.DataType == typeof(TimeSpan))
                                {
                                    if (dtRow[dcol.ColumnName] == DBNull.Value) continue;

                                    if (dcol.DataType == typeof(TimeSpan))
                                        dtRow[dcol.ColumnName] = Utils.ConvertToDateTimeSpan(dtRow[dcol.ColumnName]);
                                    else
                                        dtRow[dcol.ColumnName] = Utils.ConvertToDateTime(dtRow[dcol.ColumnName]);
                                }
                                else if (dcol.DataType == typeof(string))
                                {
                                    dtRow[dcol.ColumnName] = Utils.DecryptCombined(Convert.ToString(dtRow[dcol.ColumnName]), Utils.FixedSaltKey);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception x) { this.Write_log_file("ExecuteDataSet", x.Message); }

            return Ds;
        }

        public DataTable ExecuteDataTable(SqlConnection oCon, string sSql)
        {
            DataTable Dt = null;

            DataSet Ds = this.ExecuteDataSet(oCon, sSql);
            if (Ds != null && Ds.Tables.Count > 0)
            {
                Dt = Ds.Tables[0];
            }

            return Dt;
        }

        public DataTable ExecuteStoreProcDataTable(SqlConnection oCon, string sSql, Parameter[] paramList)
        {
            if (oCon == null || oCon.State != ConnectionState.Open) return null;

            DataTable Dt = null;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.CommandText = sSql;
                comm.CommandType = CommandType.StoredProcedure;
                comm.Connection = oCon;

                if (paramList != null)
                {
                    foreach (Parameter param in paramList)
                    {
                        SqlParameter parameter = new SqlParameter();
                        parameter.ParameterName = "@" + param.ParameterName;
                        parameter.Value = param.ParameterValue;
                        parameter.DbType = param.Type;
                        parameter.Direction = param.Direction;
                        comm.Parameters.Add(parameter);
                    }
                }

                SqlDataAdapter Da = new SqlDataAdapter(comm);
                DataSet Ds = new DataSet();
                Da.Fill(Ds);

                if (Ds != null && Ds.Tables.Count > 0)
                {
                    Dt = Ds.Tables[0];
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        foreach (DataColumn dcol in Dt.Columns)
                        {
                            if (dcol.DataType == typeof(DateTime) || dcol.DataType == typeof(TimeSpan))
                            {
                                foreach (DataRow dtRow in Dt.Rows)
                                {
                                    if (dcol.DataType == typeof(TimeSpan))
                                        dtRow[dcol.ColumnName] = Utils.ConvertToDateTimeSpan(dtRow[dcol.ColumnName]);
                                    else
                                        dtRow[dcol.ColumnName] = Utils.ConvertToDateTime(dtRow[dcol.ColumnName]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception x)
            {
                Dt = null;
            }

            return Dt;
        }

        public SqlDataReader ExecuteStoreProc(SqlConnection oCon, string xiStoreProcName, Parameter[] xiParameterList)
        {
            SqlDataReader readerObject = null;

            try
            {
                SqlCommand commandObject = new SqlCommand();
                commandObject.CommandText = xiStoreProcName;
                commandObject.CommandType = CommandType.StoredProcedure;
                commandObject.Connection = oCon;

                if (xiParameterList != null)
                {
                    foreach (Parameter xiParameterObject in xiParameterList)
                    {
                        SqlParameter parameter = new SqlParameter();
                        parameter.ParameterName = "@" + xiParameterObject.ParameterName;
                        parameter.Value = xiParameterObject.ParameterValue;
                        parameter.DbType = xiParameterObject.Type;
                        parameter.Direction = xiParameterObject.Direction;
                        commandObject.Parameters.Add(parameter);
                    }
                }

                readerObject = commandObject.ExecuteReader();

                if (commandObject.Parameters != null && xiParameterList != null)
                {
                    for (int i = 0; i < commandObject.Parameters.Count; i++)
                    {
                        SqlParameter param = commandObject.Parameters[i] as SqlParameter;
                        xiParameterList[i].ParameterValue = param.Value;
                    }
                }
            }
            catch
            {
                readerObject = null;
            }
            finally
            {
            }

            return readerObject;
        }

        public object GetValue(SqlDataReader xiReader, string xiColumname)
        {
            if (xiReader == null) return 0;

            object obj = null;
            try
            {
                obj = xiReader.GetValue(xiReader.GetOrdinal(xiColumname));
                if (obj == null || obj == DBNull.Value)
                {
                    obj = null;
                    return obj;
                }

                if (obj.GetType() == typeof(decimal))
                {
                    obj = Convert.ToDouble(obj, new System.Globalization.CultureInfo("en-US"));

                }
                else if (obj.GetType() == typeof(DateTime))
                {
                    obj = Utils.ConvertToDateTime(obj);
                }
                else if (obj.GetType() == typeof(TimeSpan))
                {
                    obj = Utils.ConvertToDateTimeSpan(obj);
                }
                else if (obj.GetType() == typeof(string))
                {
                    obj = Utils.DecryptCombined(Convert.ToString(obj), Utils.FixedSaltKey);
                }
            }
            catch
            {
                obj = null;
            }
            return obj;
        }

        public void Connectdb()
        {
            try
            {
                if (con == null || con.State != ConnectionState.Open)
                {
                    con = new SqlConnection();
                    con.ConnectionString = connectionString;
                    con.Open();
                }
            }
            catch (Exception x)
            {
                this.Write_log_file("Connectdb", x.Message);
            }
        }

        public void Disconnectdb()
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
            con = null;
        }

        public void Write_log_file(string sfunc, string sMsg)
        {
            string path = Path.Combine(ApplicationBasePath, "Log");
            StreamWriter SW = null;
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                SW = new StreamWriter(Path.Combine(path, "logs.txt"), true);
                SW.WriteLine(sfunc + "  >> " + sMsg + " >> " + DateTime.Now.ToString());
            }
            catch { }
            finally
            {
                if (SW != null)
                {
                    SW.Close();
                    SW.Dispose();
                }
                SW = null;
            }
        }

    }

    public class Parameter
    {
        private string parameterName = string.Empty;
        private object parameterValue = null;
        private DbType dbType = DbType.String;
        private ParameterDirection direction = ParameterDirection.Input;
        private int parameterSize = 0;
        string[] cultures = { "de-DE", "es-US", "en-GB", "zh-CN", "ms-MY", "ru-RU", "th-TH", "tr-TR", "vi-VN", "it-IT", "zh-TW", "en-ZA", "hu-HU", "fr-CA", "ar-AE", "es-ES", "ja-JP", "ko-KR", "bn-BD", "pt-BR", "fr-FR" };

        #region Properties

        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        public object ParameterValue
        {
            get { return parameterValue; }
            set { parameterValue = value; }
        }

        public DbType Type
        {
            get { return dbType; }
            set { dbType = value; }
        }

        public ParameterDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public int ParameterSize
        {
            get { return parameterSize; }
            set { parameterSize = value; }
        }

        #endregion

        public Parameter()
        {
        }

        public Parameter(string xiName, object xiValue)
        {
            this.parameterName = xiName;
            if (xiValue == null || xiValue == DBNull.Value || xiValue.ToString().Length == 0)
            {
                this.parameterValue = DBNull.Value;
            }
            else
            {
                this.parameterValue = xiValue;
            }
        }

        public Parameter(string xiName, object xiValue, DbType xiDbType)
        {
            this.parameterName = xiName;

            if (xiValue == null || xiValue == DBNull.Value || xiValue.ToString().Length == 0)
            {
                this.parameterValue = DBNull.Value;
            }
            else
            {
                if (xiDbType == DbType.DateTime || xiDbType == DbType.Time)
                {
                    DateTime tempDate = DateTime.MinValue;
                    foreach (string c in cultures)
                    {
                        try
                        {
                            CultureInfo info = new CultureInfo(c);
                            tempDate = Convert.ToDateTime(xiValue, info);
                        }
                        catch { }
                        if (tempDate != DateTime.MinValue) break;
                    }

                    if (tempDate == DateTime.MinValue)
                    {
                        this.parameterValue = DBNull.Value;
                    }
                    else
                    {
                        TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(Utils.Timezone);
                        tempDate = TimeZoneInfo.ConvertTime(tempDate, timezone, TimeZoneInfo.Utc);
                        this.parameterValue = tempDate;
                    }
                }
                else
                    this.parameterValue = xiValue;
            }

            this.dbType = xiDbType;
        }

        public Parameter(string xiName, object xiValue, ParameterDirection xiDirection)
        {
            this.parameterName = xiName;
            this.parameterValue = xiValue;
            this.direction = xiDirection;
        }

        public Parameter(string xiName, object xiValue, DbType xiDbType, ParameterDirection xiDirection)
        {
            this.parameterName = xiName;
            this.parameterValue = xiValue;
            this.dbType = xiDbType;
            this.direction = xiDirection;
        }
    }
}
