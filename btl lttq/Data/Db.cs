using System;
using System.Configuration;
using System.Data.SqlClient;

namespace btl_lttq.Data
{
    public static class Db
    {
        private static string ConnStr =>
            ConfigurationManager.ConnectionStrings["MessengerDb"].ConnectionString;

        public static SqlConnection OpenConn()
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();
            return conn;
        }
    }
}
