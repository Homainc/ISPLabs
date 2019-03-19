using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ISPLabs.Services
{
    public class OracleHelper
    {
        public static void InitDB()
        {
            var conn = GetDBConnection();
            conn.Open();
            try
            {
                CallSQLScript(conn, "TABLE_EXIST");
                if (GetResultFromBoolFunc(conn, "TABLE_EXIST"))
                    CallSQLScript(conn, "DROP_TABLES");
                CallSQLScript(conn, "CREATE_TABLES");
                CallSQLScript(conn, "INSERT_INIT_ROLES");
                CallProcedureAsync(conn, "INSERT_INIT_ROLES");
                InitFunctions();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static OracleCommand SetupProcCmd(string proc, OracleConnection conn, bool isFunction = true)
        {
            var cmd = new OracleCommand(proc, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            if (isFunction)
                cmd.Parameters.Add("result", OracleDbType.Int32).Direction = ParameterDirection.ReturnValue;
            return cmd;
        }

        public static bool BoolResult(OracleCommand cmd) => cmd.Parameters["result"].Value.ToString() == "1";

        public static bool BoolResultWithError(OracleCommand cmd, out string error)
        {
            if(cmd.Parameters["result"].Value.ToString() == "1")
            {
                error = null;
                return true;
            }
            else
            {
                error = cmd.Parameters["er"].Value.ToString();
                return false;
            }
        }

        public static OracleConnection GetDBConnection()
        {
            var host = "localhost";
            var port = 1521;
            var sid = "xe";
            var password = "1234";
            var user = "forum";
            string connStr = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                 + host + ")(PORT = " + port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                 + sid + ")));Password=" + password + ";User ID=" + user;
            var conn = new OracleConnection(connStr);
            return conn;
        }

        public static bool GetResultFromBoolFunc(OracleConnection conn, string funcCall)
        {
            if (conn.State != ConnectionState.Open)
                throw new Exception("Connection not open");
            var cmd = new OracleCommand(funcCall, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            var resultParam = new OracleParameter("@Result", OracleDbType.Int16);
            resultParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(resultParam);
            cmd.ExecuteNonQuery();
            int result = 0;
            if (resultParam.Value != DBNull.Value)
                result = Int16.Parse(resultParam.Value.ToString());
            return result != 0;
        }

        public static void InitFunctions()
        {
            var conn = GetDBConnection();
            conn.Open();
            try
            {
                CallSQLScript(conn, "ENCRYPT_TEXT");
                CallSQLScript(conn, "UPDATE_TOPIC");
                CallSQLScript(conn, "INSERT_CATEGORY");
                CallSQLScript(conn, "DELETE_CATEGORY");
                CallSQLScript(conn, "UPDATE_CATEGORY");
                CallSQLScript(conn, "UPDATE_PARTITION");
                CallSQLScript(conn, "INSERT_PARTITION");
                CallSQLScript(conn, "DELETE_PARTITION");
                CallSQLScript(conn, "DELETE_TOPIC");
                CallSQLScript(conn, "UPDATE_MESSAGE");
                CallSQLScript(conn, "DELETE_MESSAGE");
                CallSQLScript(conn, "GET_TOPIC_WITH_USER");
                CallSQLScript(conn, "GET_TOPIC_EAGER");
                CallSQLScript(conn, "INSERT_MESSAGE");
                CallSQLScript(conn, "INSERT_TOPIC");
                CallSQLScript(conn, "GET_CATEGORY");
                CallSQLScript(conn, "GET_ROLE_BY_NAME");
                CallSQLScript(conn, "REGISTRATION");
                CallSQLScript(conn, "TABLE_EXIST");
                CallSQLScript(conn, "GET_USERS");
                CallSQLScript(conn, "GET_PARTITIONS_EAGER");
                CallSQLScript(conn, "LOGIN");
                CallSQLScript(conn, "REGISTRATION");
                CallSQLScript(conn, "GET_USER_BY_EMAIL");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static void CallSQLScript(OracleConnection conn, string scriptFileName)
        {
            if (conn.State != ConnectionState.Open)
                throw new Exception("Connection not open");
            var cmd = new OracleCommand("", conn);
            var script = File.ReadAllLines($"{Environment.CurrentDirectory}/SQL/{scriptFileName}.sql");
            int lineNum = 0;
            string currLine = "";
            var begins = new Stack<bool>();
            var oneCmd = new StringBuilder();
            var is_is = false;
            try
            {
                foreach (string line in script)
                {
                    lineNum++;
                    currLine = line.Trim();
                    if (currLine.ToUpper() == "IS" || currLine.ToUpper() == "DECLARE" || currLine.ToUpper() == "AS")
                        is_is = true;
                    if (currLine.ToUpper() == "BEGIN")
                    {
                        begins.Push(true);
                        is_is = false;
                    }
                    if (currLine.ToUpper() == "END;")
                        begins.Pop();
                    if (currLine == "/")
                        continue;
                    if (currLine.EndsWith(";") && begins.Count == 0 && !is_is)
                    {
                        if(currLine.ToUpper().EndsWith("END;"))
                            oneCmd.Append(currLine);
                        else
                            oneCmd.Append(currLine.Substring(0, currLine.Length - 1));
                        cmd.CommandText = oneCmd.ToString();
                        cmd.ExecuteNonQuery();
                        oneCmd.Clear();
                    }
                    else
                        oneCmd.Append(currLine + " ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at line {lineNum}: {ex.Message}\n{oneCmd}");
            }
        }

        public async static void CallProcedureAsync(OracleConnection conn, string proc)
        {
            var cmd = OracleHelper.SetupProcCmd(proc, conn, false);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
