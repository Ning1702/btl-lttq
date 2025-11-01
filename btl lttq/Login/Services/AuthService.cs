using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace btl_lttq.Login.Services
{
    public static class AuthService
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["MessengerDb"].ConnectionString;

        public static bool Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT PasswordHash FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return false;

                    // Nếu là VARBINARY (ví dụ 0x313233343536)
                    if (result is byte[] bytes)
                    {
                        string dbPassword = Encoding.UTF8.GetString(bytes);
                        return dbPassword == password;
                    }
                    else
                    {
                        string dbPassword = result.ToString();
                        return dbPassword == password;
                    }
                }
            }
        }

        public static bool Register(string email, string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string check = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                SqlCommand checkCmd = new SqlCommand(check, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);
                int exists = (int)checkCmd.ExecuteScalar();
                if (exists > 0) return false;

                // Lưu password dạng VARBINARY
                byte[] pwBytes = Encoding.UTF8.GetBytes(password);

                string insert = @"INSERT INTO Users (Email, UserName, PasswordHash)
                                  VALUES (@Email, @UserName, @PasswordHash)";
                SqlCommand insertCmd = new SqlCommand(insert, conn);
                insertCmd.Parameters.AddWithValue("@Email", email);
                insertCmd.Parameters.AddWithValue("@UserName", username);
                insertCmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary).Value = pwBytes;
                insertCmd.ExecuteNonQuery();

                return true;
            }
        }
    }
}
