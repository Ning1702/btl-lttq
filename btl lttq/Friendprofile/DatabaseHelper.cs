using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace btl_lttq
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["MessengerDb"].ConnectionString;

        // 🔹 Kiểm tra đăng nhập và lấy UserId
        public static Guid GetUserIdByEmailAndPassword(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT Id 
                               FROM Users 
                               WHERE Email = @Email 
                                 AND PasswordHash = CONVERT(VARBINARY(256), @Password)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    var result = cmd.ExecuteScalar();
                    return result != null ? (Guid)result : Guid.Empty;
                }
            }
        }

        // 🔹 Lấy tên hiển thị của người dùng
        public static string GetDisplayName(Guid userId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT DisplayName FROM Users WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    var obj = cmd.ExecuteScalar();
                    return obj?.ToString() ?? "(Không tên)";
                }
            }
        }

       
        // 🔹 Lấy danh sách bạn bè
        public static List<FriendInfo> GetFriends(Guid userId)
        {
            List<FriendInfo> list = new List<FriendInfo>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetFriendsByUserId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    list.Add(new FriendInfo
                    {
                        FriendId = r.GetGuid(r.GetOrdinal("FriendId")),
                        FriendName = r["FriendName"].ToString(),
                        AvatarUrl = r["AvatarUrl"] == DBNull.Value ? "default.png" : r["AvatarUrl"].ToString(),
                        StatusText = r["StatusText"] == DBNull.Value ? "" : r["StatusText"].ToString(),
                        FriendUsername = r["FriendUsername"].ToString() // ✅ lấy username
                    });
                }
            }
            return list;
        }

        // 🔹 Lấy thông tin hồ sơ người dùng theo UserId
        public static DataRow GetUserProfile(Guid userId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetUserProfile", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                }
            }
        }


    }

    public class FriendInfo
    {
        public Guid FriendId { get; set; }
        public string FriendName { get; set; }
        public string AvatarUrl { get; set; }
        public string StatusText { get; set; }
        public string FriendUsername { get; set; }
    }
}
