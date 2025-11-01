using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace btl_lttq.Friendprofile
{
    public partial class AddFriendForm : Form
    {
        private List<FriendRequest> allRequests = new List<FriendRequest>();
        private string connectionString = "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;";
        private FriendListForm _friendListForm; // 🔹 Form danh sách bạn bè để quay lại

        // ✅ Constructor nhận tham chiếu FriendListForm
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

            // Placeholder cho ô tìm kiếm
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = "Tìm lời kết bạn";
            txtSearch.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            txtSearch.GotFocus += RemovePlaceholder;
            txtSearch.LostFocus += AddPlaceholder;

            // Click ra ngoài -> bỏ focus, vẫn cho phép focus vào TextBox
            AttachClearFocusHandlers(this);

            // Nếu bạn ADD động các panel vào flowRequests, gắn handler cho control mới
            flowRequests.ControlAdded += (s, ev) => AttachClearFocusHandlers(ev.Control);
        }

        // helper: gắn ClearFocus cho toàn bộ cây control (trừ TextBox)
        private void AttachClearFocusHandlers(Control root)
        {
            void ClearFocus(object s, EventArgs ev)
            {
                if (!(s is TextBox))
                    this.ActiveControl = null;
            }

            root.Click -= ClearFocus; // tránh nhân handler
            root.Click += ClearFocus;

            foreach (Control child in root.Controls)
                AttachClearFocusHandlers(child);
        }

        // placeholder handlers
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



        // 🔹 Load danh sách lời mời kết bạn (Status = 0)
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
                            u.Id AS SenderId
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
                                SenderId = reader.GetGuid(3)
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

                // Bo tròn avatar
                avatar.Paint += (s, e) =>
                {
                    System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                    gp.AddEllipse(0, 0, avatar.Width - 1, avatar.Height - 1);
                    avatar.Region = new Region(gp);
                };

                // Label tên
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
                btnAccept.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnAccept.Location = new Point(p.Width - 190, 20);
                btnAccept.Click += (s, e) => AcceptRequest(req.FriendshipId);

                // Nút “Xóa”
                Button btnDelete = new Button();
                btnDelete.Text = "Xóa";
                btnDelete.Font = new Font("Segoe UI", 9);
                btnDelete.ForeColor = Color.White;
                btnDelete.BackColor = Color.LightCoral;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.Size = new Size(80, 30);
                btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnDelete.Location = new Point(p.Width - 90, 20);
                btnDelete.Click += (s, e) => DeleteRequest(req.FriendshipId);

                p.Controls.Add(avatar);
                p.Controls.Add(lblName);
                p.Controls.Add(btnAccept);
                p.Controls.Add(btnDelete);
                flowRequests.Controls.Add(p);
            }
        }

        // 🔹 Khi người dùng chấp nhận lời mời
        private void AcceptRequest(Guid friendshipId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Cập nhật trạng thái lời mời sang accepted
                    string updateSql = "UPDATE Friendships SET Status = 1, UpdatedAt = SYSDATETIME() WHERE Id = @id";
                    SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@id", friendshipId);
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Đã chấp nhận lời mời kết bạn!");

                // Làm mới danh sách lời mời
                LoadFriendRequests();

                // Nếu có form FriendList -> reload lại danh sách bạn
                if (_friendListForm != null && !_friendListForm.IsDisposed)
                {
                    _friendListForm.ReloadFriends();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi chấp nhận: " + ex.Message);
            }
        }

        // 🔹 Khi người dùng xóa lời mời
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

        // 🔹 Nút “Bạn bè” → quay lại form FriendList
        private void btnFriend_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (_friendListForm != null && !_friendListForm.IsDisposed)
            {
                _friendListForm.Show();
                _friendListForm.ReloadFriends(); // cập nhật lại danh sách ngay khi quay lại
            }
            else
            {
                FriendListForm newFriendList = new FriendListForm();
                newFriendList.Show();
            }

            this.Close();
        }
    }

    // 🔹 Model lưu thông tin lời mời
    public class FriendRequest
    {
        public Guid FriendshipId { get; set; }
        public Guid SenderId { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
