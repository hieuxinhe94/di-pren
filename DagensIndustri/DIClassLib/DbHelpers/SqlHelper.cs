using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

namespace DIClassLib.DbHelpers
{
    public static class SqlHelper
    {

        


        /// <summary>
        /// ExecuteReader        
        /// </summary>
        /// <param name="connStrName">If null, connectionstring DISEMiscDB will be used</param>
        /// <param name="spName">Name of the stored procedure to execute</param>
        /// <returns>An SqlDataReader. If an error occurs, throw exception</returns>
        public static SqlDataReader ExecuteReader(string connStrName, string spName)
        {
            //DISEMiscDB is default
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(GetConnStr(connStrName));
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(spName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                /// With CommandBehavior.CloseConnection the connection will be closed when the associated DataReader is closed.
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();

                throw;
            }
        }

        /// <summary>
        /// ExecuteReader        
        /// </summary>
        /// <param name="connStrName">If null, connectionstring DISEMiscDB will be used</param>
        /// <param name="spName">Name of the stored procedure to execute</param>
        /// <param name="sqlParameter">Sql parameter passed to stored procedure</param>
        /// <returns>An SqlDataReader. If an error occurs, throw exception</returns>
        public static SqlDataReader ExecuteReader(string connStrName, string spName, SqlParameter sqlParameter)
        {
            //DISEMiscDB is default
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(GetConnStr(connStrName));
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(spName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameter != null)
                    command.Parameters.Add(sqlParameter);

                /// With CommandBehavior.CloseConnection the connection will be closed when the associated DataReader is closed.
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();

                throw;
            }
        }

        /// <summary>
        /// ExecuteReader        
        /// </summary>
        /// <param name="connStrName">If null, connectionstring DISEMiscDB will be used</param>
        /// <param name="spName">Name of the stored procedure to execute</param>
        /// <param name="sqlParameters">Array of sql parameters passed to stored procedure</param>
        /// <returns>An SqlDataReader. If an error occurs, throw exception</returns>
        public static SqlDataReader ExecuteReader(string connStrName, string spName, SqlParameter[] sqlParameters)
        {
            //DISEMiscDB is default
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(GetConnStr(connStrName));
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(spName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                        command.Parameters.Add(param);
                }

                /// With CommandBehavior.CloseConnection the connection will be closed when the associated DataReader is closed.
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();

                throw;
            }
        }

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="connStrName">If null, connectionstring DISEMiscDB will be used</param>
        /// <param name="spName">Name of the stored procedure to execute</param>
        /// <param name="sqlParameter">Sql parameter passed to stored procedure</param>
        /// <returns>A dataset. If an error occurs, throw exception</returns>
        public static DataSet ExecuteDataset(string connStrName, string spName, SqlParameter sqlParameter)
        {
            //DISEMiscDB is default
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(GetConnStr(connStrName));
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(spName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameter != null)
                    command.Parameters.Add(sqlParameter);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();
            }
        }


        public static DataSet ExecuteDatasetParam(string connStrName, string spName, SqlParameter[] sqlParameters)
        {
            //DISEMiscDB is default
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(GetConnStr(connStrName));
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(spName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                        command.Parameters.Add(param);
                }

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="connStrName">If null, connectionstring DISEMiscDB will be used</param>
        /// <param name="spName">Name of the stored procedure to execute</param>
        /// <param name="sqlParameters">Array of sql parameters passed to stored procedure</param>
        /// <returns>The number of affected rows. If an error occurs, throw exception</returns>
        public static int ExecuteNonQuery(string connStrName, string spName, SqlParameter[] sqlParameters)
        {
            //DISEMiscDB is default
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(GetConnStr(connStrName));
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(spName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                        command.Parameters.Add(param);
                }

                return command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();
            }

        }

        /// <summary>
        /// Execute plain SQL
        /// </summary>
        /// <param name="connStrName">Connectionstring name</param>
        /// <param name="sqlToExecute">Complete SQL to execute. Make sure it is not injected with bad code</param>
        public static void ExecuteNonQuerySql(string connStrName, string sqlToExecute)
        {
            using(var sqlConnection = new SqlConnection(GetConnStr(connStrName))){
                sqlConnection.Open();
                using (var command = new SqlCommand(sqlToExecute, sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// ExecuteScalar        
        /// </summary>
        /// <param name="connStrName">If null, connectionstring DISEMiscDB will be used</param>
        /// <param name="spName">Name of the stored procedure to execute</param>
        /// <param name="sqlParameter">Sql parameter passed to stored procedure</param>
        /// <returns>An objecet. If an error occurs, throw exception</returns>
        public static object ExecuteScalar(string connStrName, string spName, SqlParameter sqlParameter)
        {
            //DISEMiscDB is default
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(GetConnStr(connStrName));
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(spName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameter != null)
                    command.Parameters.Add(sqlParameter);

                return command.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();
            }
        }

        public static object ExecuteScalarParam(string connStrName, string spName, SqlParameter[] sqlParameters)
        {
            //DISEMiscDB is default
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(GetConnStr(connStrName));
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(spName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter param in sqlParameters)
                        command.Parameters.Add(param);
                }

                return command.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();
            }
        }


        private static string GetConnStr(string connStrName)
        {
            string fallBack = System.Configuration.ConfigurationManager.ConnectionStrings["DISEMiscDB"].ToString();

            if (string.IsNullOrEmpty(connStrName))
                return fallBack;

            try   { return System.Configuration.ConfigurationManager.ConnectionStrings[connStrName].ToString(); }
            catch { return fallBack; }
        }

    }
}