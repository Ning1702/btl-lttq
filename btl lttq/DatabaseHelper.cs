using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace btl_lttq
{
    public static class DatabaseHelper
    {
        private static string connectionString =
    "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;";

        public static List<FriendInfo> GetFriends(Guid userId)
        {
            List<FriendInfo> list = new List<FriendInfo>();
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                        FriendId = r.GetGuid(0),
                        FriendName = r.GetString(1),
                        AvatarUrl = r.IsDBNull(2) ? "default.png" : r.GetString(2),
                        StatusText = r.IsDBNull(3) ? "" : r.GetString(3)
                    });
                }
            }
            return list;
        }
    }

    public class FriendInfo
    {
        public Guid FriendId { get; set; }
        public string FriendName { get; set; }
        public string AvatarUrl { get; set; }
        public string StatusText { get; set; }
    }
}
