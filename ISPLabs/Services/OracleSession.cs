using Oracle.ManagedDataAccess.Client;
using System;

namespace ISPLabs.Services
{
    public class OracleSession : IDisposable
    {
        private OracleConnection _conn;
        private bool _disposed;

        public OracleConnection Connection { get { return _conn; } }

        public OracleSession()
        {
            var host = "localhost";
            var port = 1521;
            var sid = "xe";
            var password = "1234";
            var user = "forum";
            string connStr = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                 + host + ")(PORT = " + port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                 + sid + ")));Password=" + password + ";User ID=" + user;
            _conn = new OracleConnection(connStr);
            _conn.Open();
        }

        public void AddLoginContext(string username)
        {
            var cmd = OracleHelper.SetupProcCmd("LOGIN_CONTEXT_ADD", _conn, false);
            cmd.Parameters.Add("username", OracleDbType.Varchar2, 255).Value = username;
            cmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _conn.Close();
                    _conn.Dispose();
                }
                _disposed = true;
            }
        }

        ~OracleSession()
        {
            Dispose(false);
        }
    }
}
