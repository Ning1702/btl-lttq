using btl_lttq.Data; // dùng FriendInfo & DatabaseHelper trong namespace Data
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace btl_lttq.Friendprofile
{
    public partial class FriendListForm : Form
    {
        private List<FriendInfo> allFriends = new List<FriendInfo>();

        public FriendListForm()
        {
            InitializeComponent();
        }

        private void FriendListForm_Load(object sender, EventArgs e)
        {
            // --- Placeholder cho ô tìm kiếm ---
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = "Tìm kiếm bạn bè";
            txtSearch.Font = new Font("Segoe UI", 12, FontStyle.Italic);

            txtSearch.GotFocus += RemoveText;
            txtSearch.LostFocus += AddText;

            // Khi mất focus mà ô tìm kiếm trống → hiển thị lại toàn bộ
            txtSearch.LostFocus += (s, e2) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text) || txtSearch.Text == "Tìm kiếm bạn bè")
                {
                    DisplayFriends(allFriends);
                }
            };

            Guid currentUserId = GetUserId("anninh");
            allFriends = DatabaseHelper.GetFriends(currentUserId);
            DisplayFriends(allFriends);

         

            // Nút Thêm bạn
            btnAddFriend.Click += (_, __) =>
            {
                var addForm = new AddFriendForm(this);
                addForm.StartPosition = FormStartPosition.CenterScreen;
                addForm.FormClosed += (s2, e2) => this.Show();
                this.Hide();
                addForm.Show();
            };

            this.Click += (_, __) => this.ActiveControl = null;
            panelHeader.Click += (_, __) => this.ActiveControl = null;
            flowFriends.Click += (_, __) => this.ActiveControl = null;

            this.ActiveControl = null;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.ActiveControl = null;
        }
        public void ReloadFriends()
        {
            LoadFriends();
        }

        private void LoadFriends()
        {
            try
            {
                Guid currentUserId = GetUserId("anninh");
                allFriends = DatabaseHelper.GetFriends(currentUserId);
                DisplayFriends(allFriends);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải danh sách bạn: " + ex.Message);
            }
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm bạn bè")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Tìm kiếm bạn bè";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void DisplayFriends(List<FriendInfo> friends)
        {
            flowFriends.Controls.Clear();
            flowFriends.Padding = new Padding(10);

            foreach (var f in friends)
            {
                var p = new Panel
                {
                    Width = flowFriends.Width - 30,
                    Height = 70,
                    Margin = new Padding(0, 0, 0, 10),
                    BackColor = Color.WhiteSmoke
                };

                // Avatar
                var avatar = new PictureBox
                {
                    Size = new Size(50, 50),
                    Location = new Point(10, 10),
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                string path = Path.Combine(Application.StartupPath, "Images", f.AvatarUrl ?? "");
                if (File.Exists(path)) avatar.Image = Image.FromFile(path);
                else avatar.BackColor = Color.LightGray;

                // Bo tròn avatar
                avatar.Paint += (s, e) =>
                {
                    var gp = new System.Drawing.Drawing2D.GraphicsPath();
                    gp.AddEllipse(0, 0, avatar.Width - 1, avatar.Height - 1);
                    avatar.Region = new Region(gp);
                };

                // Tên bạn bè
                var lblName = new Label
                {
                    Text = f.FriendName,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(70, 10)
                };

                // Trạng thái
                var lblStatus = new Label
                {
                    Text = f.StatusText,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(70, 35)
                };

                // Nút “Nhắn tin”
                var btnChat = new Button
                {
                    Text = "Nhắn tin",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.White,
                    BackColor = Color.RoyalBlue,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(90, 30),
                    Location = new Point(p.Width - 270, 20),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnChat.FlatAppearance.BorderSize = 0;
                btnChat.FlatAppearance.MouseOverBackColor = Color.DodgerBlue;
                btnChat.Click += (s, e) => MessageBox.Show($"💬 Mở chat với {f.FriendName}");

                // Nút “Thông tin” (ở giữa)
                var btnInfo = new Button
                {
                    Text = "Thông tin",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.White,
                    BackColor = Color.MediumSeaGreen,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(90, 30),
                    Location = new Point(p.Width - 180, 20),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnInfo.FlatAppearance.BorderSize = 0;
                btnInfo.FlatAppearance.MouseOverBackColor = Color.SeaGreen;

                btnInfo.Click += (s, e) =>
                {
                    try
                    {
                        // ✅ Mở form hồ sơ bạn bè (ProfileFriendForm)
                        var profileForm = new ProfileFriendForm(f.FriendId);
                        profileForm.StartPosition = FormStartPosition.CenterScreen;
                        profileForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("❌ Không thể mở thông tin bạn bè: " + ex.Message);
                    }
                };



                // Nút “Xóa bạn”
                var btnDelete = new Button
                {
                    Text = "Xóa bạn",
                    Font = new Font("Segoe UI", 9),
                    BackColor = Color.LightCoral,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(80, 30),
                    Location = new Point(p.Width - 90, 20),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.FlatAppearance.MouseOverBackColor = Color.IndianRed;

                btnDelete.Click += (s, e) =>
                {
                    var confirm = MessageBox.Show(
                        $"Bạn có chắc muốn xóa bạn {f.FriendName} không?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        try
                        {
                            using (var conn = new SqlConnection(
                                "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
                            {
                                conn.Open();
                                string sql = @"
                                    DELETE FROM Friendships
                                    WHERE (RequesterId = @userId AND AddresseeId = @friendId)
                                       OR (RequesterId = @friendId AND AddresseeId = @userId)";
                                using (var cmd = new SqlCommand(sql, conn))
                                {
                                    cmd.Parameters.AddWithValue("@userId", GetUserId("anninh"));
                                    cmd.Parameters.AddWithValue("@friendId", f.FriendId);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            flowFriends.Controls.Remove(p);
                            MessageBox.Show($"{f.FriendName} đã bị xóa khỏi danh sách bạn bè.", "Thành công");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi xóa bạn: " + ex.Message);
                        }
                    }
                };

                // Thêm tất cả control vào panel
                p.Controls.Add(avatar);
                p.Controls.Add(lblName);
                p.Controls.Add(lblStatus);
                p.Controls.Add(btnChat);
                p.Controls.Add(btnInfo);
                p.Controls.Add(btnDelete);

                flowFriends.Controls.Add(p);
            }
        }

        private Guid GetUserId(string username)
        {
            using (var conn = new SqlConnection(
                "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
            {
                var cmd = new SqlCommand("SELECT Id FROM Users WHERE UserName=@u", conn);
                cmd.Parameters.AddWithValue("@u", username);
                conn.Open();
                return (Guid)cmd.ExecuteScalar();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword) || txtSearch.Text == "Tìm kiếm bạn bè")
            {
                DisplayFriends(allFriends);
                return;
            }

            string keywordNoDiacritics = RemoveDiacritics(keyword);

            var filtered = allFriends
                .Where(f =>
                {
                    string name = f.FriendName?.ToLower() ?? "";
                    string nameNoDiacritics = RemoveDiacritics(name);
                    return name.Contains(keyword) || nameNoDiacritics.Contains(keywordNoDiacritics);
                })
                .ToList();

            DisplayFriends(filtered);

            if (filtered.Count == 0)
            {
                MessageBox.Show("Không tìm thấy bạn nào phù hợp.", "Thông báo");
            }
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            string normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in normalized)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(c);
                if (cat != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
