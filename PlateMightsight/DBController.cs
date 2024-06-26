using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateMightsight
{
    public class DBController : IDisposable
    {
        public SqlConnection conn;

        public string ConnectionString { get; set; }

        public DBController(string dbServer, string dbName, string dbUserID, string dbPassword)
        {
            try
            {
                ConnectionString = "Data Source=" + dbServer + ";Initial Catalog=" + dbName + ";User ID=" + dbUserID + ";Password=" + dbPassword + ";MultipleActiveResultSets=True;Connection Timeout=2";
                conn = new SqlConnection(ConnectionString);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DBController(string connectionString)
        {
            try
            {
                ConnectionString = connectionString;
                conn = new SqlConnection(ConnectionString);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void OpenConnection()
        {
            try
            {
                conn.Close();
                conn.Open();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                conn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SqlCommand GetSelectCommand(string fields, string tablename, string condition, string aggregation)
        {
            string empty = string.Empty;
            empty = ((!(condition != string.Empty)) ? ("SELECT " + fields + " FROM " + tablename + " " + aggregation) : ("SELECT " + fields + " FROM " + tablename + " WHERE " + condition + " " + aggregation));
            return new SqlCommand(empty, conn);
        }

        public SqlCommand GetInsertCommand(string tableName, string valueString)
        {
            string cmdText = "INSERT INTO " + tableName + " VALUES(" + valueString + ")";
            return new SqlCommand(cmdText, conn);
        }

        public SqlCommand GetInsertCommandReturnIdentityColumn(string tableName, string valueString, string identityColumn)
        {
            string cmdText = "INSERT INTO " + tableName + " VALUES(" + valueString + ");SELECT Scope_Identity( ) " + identityColumn;
            return new SqlCommand(cmdText, conn);
        }

        public SqlCommand GetUpdateCommand(string tableName, string updateValues, string conditions)
        {
            string cmdText = "UPDATE " + tableName + " SET " + updateValues + " WHERE " + conditions;
            return new SqlCommand(cmdText, conn);
        }

        public SqlCommand GetDeleteCommand(string tableName, string conditions)
        {
            string text = "DELETE FROM " + tableName;
            if (!conditions.Equals(string.Empty))
            {
                text = text + " WHERE " + conditions;
            }

            return new SqlCommand(text, conn);
        }

        public SqlDataAdapter GetDataAdapter(string fields, string tablename, string condition, string aggregation)
        {
            string empty = string.Empty;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            empty = ((!(condition != string.Empty)) ? ("SELECT " + fields + " FROM " + tablename + " " + aggregation) : ("SELECT " + fields + " FROM " + tablename + " WHERE " + condition + " " + aggregation));
            try
            {
                SqlCommand selectCommand = new SqlCommand(empty, conn);
                sqlDataAdapter.SelectCommand = selectCommand;
            }
            catch
            {
                return null;
            }

            return sqlDataAdapter;
        }

        public DataSet GetDataSet(string fields, string tablename, string condition, string aggregation)
        {
            string empty = string.Empty;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            empty = ((!(condition != string.Empty)) ? ("SELECT " + fields + " FROM " + tablename + " " + aggregation) : ("SELECT " + fields + " FROM " + tablename + " WHERE " + condition + " " + aggregation));
            try
            {
                SqlCommand selectCommand = new SqlCommand(empty, conn);
                sqlDataAdapter.SelectCommand = selectCommand;
                sqlDataAdapter.Fill(dataSet, tablename);
            }
            catch
            {
                return null;
            }

            return dataSet;
        }

        public DataTable GetRecord(string fields, string tablename, string condition, string aggregation, out string query)
        {
            query = string.Empty;
            if (condition != string.Empty)
            {
                query = "SELECT " + fields + " FROM " + tablename + " WHERE " + condition + " " + aggregation;
            }
            else
            {
                query = "SELECT " + fields + " FROM " + tablename + " " + aggregation;
            }

            return GetRecord(query);
        }

        public DataTable GetRecord(string fields, string tablename, string condition, string aggregation)
        {
            string empty = string.Empty;
            empty = ((!(condition != string.Empty)) ? ("SELECT " + fields + " FROM " + tablename + " " + aggregation) : ("SELECT " + fields + " FROM " + tablename + " WHERE " + condition + " " + aggregation));
            return GetRecord(empty);
        }

        public DataTable GetRecord(string fields, string tablename, string condition)
        {
            string query = "SELECT " + fields + " FROM " + tablename + " WHERE " + condition;
            return GetRecord(query);
        }

        public DataTable GetRecord(string fields, string tablename)
        {
            string query = "SELECT " + fields + " FROM " + tablename;
            return GetRecord(query);
        }

        public DataTable GetRecord(string query)
        {
            DataTable dataTable = null;
            try
            {
                return ExecuteReader(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetRecordFromStoredProcedure(string storedProcedure)
        {
            DataTable dataTable = null;
            try
            {
                return ExecuteStoredProcedure(storedProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddRecord(string tableName, string valueString)
        {
            try
            {
                string sql = "INSERT INTO " + tableName + " VALUES(" + valueString + ")";
                ExecuteNonQuery(sql);
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void AddRecord(string tableName, string valueString, out string query)
        {
            try
            {
                query = "INSERT INTO " + tableName + " VALUES(" + valueString + ")";
                ExecuteNonQuery(query);
            }
            catch (SqlException ex)
            {
                query = "INSERT INTO " + tableName + " VALUES(" + valueString + ")";
                throw new Exception(ex.Message);
            }
        }

        public void AddRecord(string tableName, string columnSet, string valueString)
        {
            try
            {
                string sql = "INSERT INTO " + tableName + " (" + columnSet + ")  VALUES(" + valueString + ")";
                ExecuteNonQuery(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddRecord(string tableName, string columnSet, string valueString, out string query)
        {
            try
            {
                query = "INSERT INTO " + tableName + " (" + columnSet + ")  VALUES (" + valueString + ")";
                ExecuteNonQuery(query);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateRecord(string tableName, string updateValues, string conditions)
        {
            try
            {
                string sql = "UPDATE " + tableName + " SET " + updateValues + " WHERE " + conditions;
                ExecuteNonQuery(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateRecord(string tableName, string updateValues, string conditions, out string query)
        {
            try
            {
                query = "UPDATE " + tableName + " SET " + updateValues + " WHERE " + conditions;
                ExecuteNonQuery(query);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteRecord(string tableName, string conditions)
        {
            try
            {
                string text = "DELETE FROM " + tableName;
                if (!conditions.Equals(string.Empty))
                {
                    text = text + " WHERE " + conditions;
                }

                ExecuteNonQuery(text);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteRecord(string tableName, string conditions, out string query)
        {
            try
            {
                query = "DELETE FROM " + tableName;
                if (!conditions.Equals(string.Empty))
                {
                    query = query + " WHERE " + conditions;
                }

                ExecuteNonQuery(query);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Connection = conn;
            SqlTransaction sqlTransaction2 = (sqlCommand.Transaction = conn.BeginTransaction());
            try
            {
                sqlCommand.ExecuteNonQuery();
                sqlTransaction2.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction2.Rollback();
                throw new Exception(ex.Message);
            }

            sqlCommand.Dispose();
        }

        protected void ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            if (parameters.Length != 0)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            sqlCommand.Connection = conn;
            SqlTransaction sqlTransaction2 = (sqlCommand.Transaction = conn.BeginTransaction());
            try
            {
                sqlCommand.ExecuteNonQuery();
                sqlTransaction2.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction2.Rollback();
                throw new Exception(ex.Message);
            }

            sqlCommand.Dispose();
        }

        protected DataTable ExecuteReader(string sql)
        {
            DataTable dataTable = null;
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Connection = conn;
            SqlTransaction sqlTransaction2 = (sqlCommand.Transaction = conn.BeginTransaction());
            try
            {
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                dataTable = new DataTable();
                dataTable.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlTransaction2.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction2.Rollback();
                throw new Exception(ex.Message);
            }

            sqlTransaction2.Dispose();
            sqlCommand.Dispose();
            return dataTable;
        }

        protected DataTable ExecuteQuery(string sql)
        {
            DataTable dataTable = null;
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Connection = conn;
            SqlTransaction sqlTransaction2 = (sqlCommand.Transaction = conn.BeginTransaction());
            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable2 = new DataTable();
                sqlDataAdapter.Fill(dataTable2);
                dataTable = dataTable2;
                sqlTransaction2.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction2.Rollback();
                throw new Exception(ex.Message);
            }

            sqlTransaction2.Dispose();
            sqlCommand.Dispose();
            return dataTable;
        }

        protected DataTable ExecuteStoredProcedure(string spName)
        {
            DataTable dataTable = null;
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = spName;
            sqlCommand.Connection = conn;
            SqlTransaction sqlTransaction2 = (sqlCommand.Transaction = conn.BeginTransaction());
            try
            {
                SqlDataReader reader = sqlCommand.ExecuteReader();
                DataTable dataTable2 = new DataTable();
                dataTable2.Load(reader);
                dataTable = dataTable2;
                sqlTransaction2.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction2.Rollback();
                throw new Exception(ex.Message);
            }

            sqlTransaction2.Dispose();
            sqlCommand.Dispose();
            return dataTable;
        }

        protected void ExecuteNonQueryStoredProcedure(string spName, params SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = spName;
            if (parameters.Length != 0)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            sqlCommand.Connection = conn;
            SqlTransaction sqlTransaction2 = (sqlCommand.Transaction = conn.BeginTransaction());
            try
            {
                sqlCommand.ExecuteNonQuery();
                sqlTransaction2.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction2.Rollback();
                throw new Exception(ex.Message);
            }

            sqlTransaction2.Dispose();
            sqlCommand.Dispose();
        }

        protected DataTable GetTableEventByID(string spName, int eventID)
        {
            DataTable dataTable = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(spName, conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = sqlCommand.Parameters.Add("@EventID", SqlDbType.Int, eventID);
                sqlParameter.Value = eventID;
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                DataTable dataTable2 = new DataTable();
                dataTable2.Load(sqlDataReader);
                sqlDataReader.Close();
                return dataTable2;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
