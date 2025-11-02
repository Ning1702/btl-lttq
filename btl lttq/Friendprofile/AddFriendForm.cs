using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Text;

namespace btl_lttq.Friendprofile
{
    public partial class AddFriendForm : Form
    {
        private List<FriendRequest> allRequests = new List<FriendRequest>();
        private string connectionString = "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;";
        private FriendListForm _friendListForm; // 🔹 Form danh sách bạn bè để quay lại

        public AddFriendForm(FriendListForm friendListForm)
        {
            InitializeComponent();
            _friendListForm = friendListForm;
        }

        private void AddFriendForm_Load(object sender, EventArgs e)
        {
            flowRequests.AutoScroll = true;
            flowRequests.WrapContents = false;
            flowRequests.FlowDirection = FlowDirection.TopDown;
            LoadFriendRequests();

            // Placeholder
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = "Tìm lời kết bạn";
            txtSearch.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            txtSearch.GotFocus += RemovePlaceholder;
            txtSearch.LostFocus += AddPlaceholder;

            txtSearch.LostFocus += (s, e2) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text) || txtSearch.Text == "Tìm lời kết bạn")
                    DisplayFriendRequests(allRequests);
            };

            // Click ra ngoài -> bỏ focus
            this.ActiveControl = null;
            this.MouseDown += (s, e) =>
            {
                Control c = this.GetChildAtPoint(e.Location);
                if (c == null || !(c is TextBox))
                    this.ActiveControl = null;
            };
            this.ActiveControl = null; //
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.ActiveControl = null; // không auto focus khi form hiển thị
        }


        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm lời kết bạn")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
                txtSearch.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            }
        }

        private void AddPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Tìm lời kết bạn";
                txtSearch.ForeColor = Color.Gray;
                txtSearch.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            }
        }

        // 🔹 Load danh sách lời mời kết bạn
        private void LoadFriendRequests()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Guid currentUserId = GetUserId("anninh");

                    string sql = @"
                        SELECT 
                            f.Id AS FriendshipId, 
                            u.DisplayName, 
                            u.AvatarUrl,
                            u.Id AS SenderId,
                            u.UserName
                        FROM Friendships f
                        JOIN Users u ON u.Id = f.RequesterId
                        WHERE f.AddresseeId = @userId AND f.Status = 0";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", currentUserId);
                        SqlDataReader reader = cmd.ExecuteReader();

                        allRequests.Clear();
                        while (reader.Read())
                        {
                            allRequests.Add(new FriendRequest
                            {
                                FriendshipId = reader.GetGuid(0),
                                DisplayName = reader.GetString(1),
                                AvatarUrl = reader.IsDBNull(2) ? null : reader.GetString(2),
                                SenderId = reader.GetGuid(3),
                                FriendUsername = reader.GetString(4)
                            });
                        }
                    }
                }

                DisplayFriendRequests(allRequests);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải lời mời: " + ex.Message);
            }
        }

        // 🔹 Hiển thị danh sách lời mời
        private void DisplayFriendRequests(List<FriendRequest> requests)
        {
            flowRequests.Controls.Clear();
            flowRequests.Padding = new Padding(10);

            foreach (var req in requests)
            {
                Panel p = new Panel();
                p.Width = flowRequests.Width - 35;
                p.Height = 70;
                p.Margin = new Padding(0, 0, 0, 10);
                p.BackColor = Color.WhiteSmoke;

                // Avatar
                PictureBox avatar = new PictureBox();
                avatar.Size = new Size(50, 50);
                avatar.Location = new Point(10, 10);
                avatar.SizeMode = PictureBoxSizeMode.Zoom;
                string path = Path.Combine(Application.StartupPath, "Images", req.AvatarUrl ?? "");
                if (File.Exists(path))
                    avatar.Image = Image.FromFile(path);
                else
                    avatar.BackColor = Color.LightGray;

                avatar.Paint += (s, e) =>
                {
                    System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                    gp.AddEllipse(0, 0, avatar.Width - 1, avatar.Height - 1);
                    avatar.Region = new Region(gp);
                };

                // Tên
                Label lblName = new Label();
                lblName.Text = req.DisplayName;
                lblName.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblName.AutoSize = true;
                lblName.Location = new Point(70, 20);

                // Nút “Chấp nhận”
                Button btnAccept = new Button();
                btnAccept.Text = "Chấp nhận";
                btnAccept.Font = new Font("Segoe UI", 9);
                btnAccept.ForeColor = Color.White;
                btnAccept.BackColor = Color.RoyalBlue;
                btnAccept.FlatStyle = FlatStyle.Flat;
                btnAccept.FlatAppearance.BorderSize = 0;
                btnAccept.Size = new Size(90, 30);
                btnAccept.Location = new Point(p.Width - 270, 20);
                btnAccept.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnAccept.Click += (s, e) => AcceptRequest(req.FriendshipId);

                // ✅ Nút “Thông tin”
                Button btnInfo = new Button();
                btnInfo.Text = "Thông tin";
                btnInfo.Font = new Font("Segoe UI", 9);
                btnInfo.ForeColor = Color.White;
                btnInfo.BackColor = Color.MediumSeaGreen;
                btnInfo.FlatStyle = FlatStyle.Flat;
                btnInfo.FlatAppearance.BorderSize = 0;
                btnInfo.Size = new Size(90, 30);
                btnInfo.Location = new Point(p.Width - 180, 20);
                btnInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnInfo.Click += (s, e) =>
                {
                    try
                    {
                        var profileForm = new ProfileFriendForm(req.SenderId);
                        profileForm.StartPosition = FormStartPosition.CenterScreen;
                        profileForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("❌ Không thể mở thông tin bạn: " + ex.Message);
                    }
                };

                // Nút “Xóa”
                Button btnDelete = new Button();
                btnDelete.Text = "Xóa";
                btnDelete.Font = new Font("Segoe UI", 9);
                btnDelete.ForeColor = Color.White;
                btnDelete.BackColor = Color.LightCoral;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.Size = new Size(80, 30);
                btnDelete.Location = new Point(p.Width - 90, 20);
                btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnDelete.Click += (s, e) => DeleteRequest(req.FriendshipId);

                // Thêm vào panel
                p.Controls.Add(avatar);
                p.Controls.Add(lblName);
                p.Controls.Add(btnAccept);
                p.Controls.Add(btnInfo);
                p.Controls.Add(btnDelete);
                flowRequests.Controls.Add(p);
            }
        }

        private void AcceptRequest(Guid friendshipId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string updateSql = "UPDATE Friendships SET Status = 1, UpdatedAt = SYSDATETIME() WHERE Id = @id";
                    SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@id", friendshipId);
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Đã chấp nhận lời mời kết bạn!");
                LoadFriendRequests();

                _friendListForm?.ReloadFriends();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi chấp nhận: " + ex.Message);
            }
        }

        private void DeleteRequest(Guid friendshipId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Friendships WHERE Id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", friendshipId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("🗑️ Đã xóa lời mời!");
                LoadFriendRequests();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi xóa lời mời: " + ex.Message);
            }
        }

        private Guid GetUserId(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id FROM Users WHERE UserName=@u", conn);
                cmd.Parameters.AddWithValue("@u", username);
                return (Guid)cmd.ExecuteScalar();
            }
        }

        private void btnFriend_Click(object sender, EventArgs e)
        {
            this.Hide();
            _friendListForm?.Show();
            _friendListForm?.ReloadFriends();
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword) || txtSearch.Text == "Tìm lời kết bạn")
            {
                DisplayFriendRequests(allRequests);
                return;
            }

            string keywordNoDiacritics = RemoveDiacritics(keyword);
            var filtered = allRequests.Where(r =>
            {
                string name = r.DisplayName?.ToLower() ?? "";
                string nameNoDiacritics = RemoveDiacritics(name);
                return name.Contains(keyword) || nameNoDiacritics.Contains(keywordNoDiacritics);
            }).ToList();

            DisplayFriendRequests(filtered);

            if (filtered.Count == 0)
                MessageBox.Show("Không tìm thấy lời mời phù hợp.", "Thông báo");
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            string normalized = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in normalized)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(c);
                if (cat != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }

    public class FriendRequest
    {
        public Guid FriendshipId { get; set; }
        public Guid SenderId { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public string FriendUsername { get; set; } // ✅ thêm để mở ProfileFriendForm
    }
}
