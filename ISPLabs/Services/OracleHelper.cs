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
                CallSQLScript(conn, "func_exist_table");
                if (!GetResultFromBoolFunc(conn, "EXIST_TABLE"))
                {
                    CallSQLScript(conn, "create_tables");
                    CallSQLScript(conn, "role");
                }
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static OracleConnection GetDBConnection()
        {
            var host = "localhost";
            var port = 1521;
            var sid = "xe";
            var password = "1234";
            var user = "system";
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
                    if (currLine == "IS" || currLine == "DECLARE")
                        is_is = true;
                    if (currLine == "BEGIN")
                    {
                        begins.Push(true);
                        is_is = false;
                    }
                    if (currLine == "END;")
                        begins.Pop();
                    if (currLine == "/")
                        continue;
                    if (currLine.EndsWith(";") && begins.Count == 0 && !is_is)
                    {
                        if(currLine.EndsWith("END;"))
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
    }
}
