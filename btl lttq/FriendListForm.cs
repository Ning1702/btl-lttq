using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace btl_lttq
{
    public partial class FriendListForm : Form
    {
        public FriendListForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Bạn có thể thêm logic tìm kiếm bạn bè ở đây
        }

        private void FriendListForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(
                    "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
                {
                    conn.Open();
                    MessageBox.Show("✅ Kết nối SQL Server thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi kết nối SQL Server:\n" + ex.Message);
            }

            // --- Placeholder cho ô tìm kiếm ---
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = "Tìm kiếm bạn bè";

            txtSearch.GotFocus += RemoveText;
            txtSearch.LostFocus += AddText;

            Guid currentUserId = GetUserId("anninh"); // ví dụ
            allFriends = DatabaseHelper.GetFriends(currentUserId);
            DisplayFriends(allFriends);

            // Khi click ra vùng trống trên form -> mất focus textbox
            this.Click += (_, __) => this.ActiveControl = null;
            panelHeader.Click += (_, __) => this.ActiveControl = null;
            flowFriends.Click += (_, __) => this.ActiveControl = null;

            btnAddFriend.Click += (_,__ ) =>
            {
                AddFriendForm addForm = new AddFriendForm();
                addForm.ShowDialog(); // mở dạng popup
            };

        }

        // Khi người dùng click vào ô — xóa placeholder
        private void RemoveText(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm bạn bè")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        // Khi người dùng rời khỏi ô — nếu rỗng thì hiện lại placeholder
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
                Panel p = new Panel();
                p.Width = flowFriends.Width - 30;
                p.Height = 70;
                p.Margin = new Padding(0, 0, 0, 10);
                p.BackColor = Color.WhiteSmoke;

                // Avatar
                PictureBox avatar = new PictureBox();
                avatar.Size = new Size(50, 50);
                avatar.Location = new Point(10, 10);
                avatar.SizeMode = PictureBoxSizeMode.Zoom;

                string path = Path.Combine(Application.StartupPath, "Images", f.AvatarUrl ?? "");
                if (File.Exists(path))
                    avatar.Image = Image.FromFile(path);
                else
                    avatar.BackColor = Color.LightGray; // nếu chưa có ảnh mặc định

                // Bo tròn avatar
                avatar.Paint += (s, e) =>
                {
                    System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                    gp.AddEllipse(0, 0, avatar.Width - 1, avatar.Height - 1);
                    avatar.Region = new Region(gp);
                };

                // Tên bạn bè
                Label lblName = new Label();
                lblName.Text = f.FriendName;
                lblName.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblName.AutoSize = true;
                lblName.Location = new Point(70, 10);

                // Trạng thái
                Label lblStatus = new Label();
                lblStatus.Text = f.StatusText;
                lblStatus.Font = new Font("Segoe UI", 9);
                lblStatus.ForeColor = Color.Gray;
                lblStatus.AutoSize = true;
                lblStatus.Location = new Point(70, 35);

                // Nút “Nhắn tin”
                Button btnChat = new Button();
                btnChat.Text = "Nhắn tin";
                btnChat.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                btnChat.ForeColor = Color.White;
                btnChat.BackColor = Color.RoyalBlue;
                btnChat.FlatStyle = FlatStyle.Flat;
                btnChat.FlatAppearance.BorderSize = 0;
                btnChat.FlatAppearance.MouseOverBackColor = Color.DodgerBlue;
                btnChat.Size = new Size(90, 30);
                btnChat.Location = new Point(p.Width - 180, 20);
                btnChat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnChat.Click += (s, e) =>
                {
                    MessageBox.Show($"💬 Mở chat với {f.FriendName}");
                };

                // Nút “Xóa bạn”
                Button btnDelete = new Button();
                btnDelete.Text = "Xóa bạn";
                btnDelete.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                btnDelete.BackColor = Color.LightCoral;
                btnDelete.ForeColor = Color.White;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.FlatAppearance.MouseOverBackColor = Color.IndianRed;
                btnDelete.Size = new Size(80, 30);
                btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnDelete.Location = new Point(p.Width - 90, 20);

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
                            using (SqlConnection conn = new SqlConnection(
                                "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
                            {
                                conn.Open();
                                string sql = @"
                                    DELETE FROM Friendships
                                    WHERE (RequesterId = @userId AND AddresseeId = @friendId)
                                       OR (RequesterId = @friendId AND AddresseeId = @userId)";
                                using (SqlCommand cmd = new SqlCommand(sql, conn))
                                {
                                    cmd.Parameters.AddWithValue("@userId", GetUserId("anninh")); // user hiện tại
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
                p.Controls.Add(btnDelete);

                // Thêm panel vào FlowLayoutPanel
                flowFriends.Controls.Add(p);
            }
        }

        private Guid GetUserId(string username)
        {
            using (SqlConnection conn = new SqlConnection(
                "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id FROM Users WHERE UserName=@u", conn);
                cmd.Parameters.AddWithValue("@u", username);
                conn.Open();
                return (Guid)cmd.ExecuteScalar();
            }
        }

        private List<FriendInfo> allFriends = new List<FriendInfo>();

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = RemoveDiacritics(txtSearch.Text.Trim().ToLower());

            if (string.IsNullOrEmpty(keyword))
            {
                DisplayFriends(allFriends);
                return;
            }

            var filtered = allFriends
                .Where(f => RemoveDiacritics(f.FriendName.ToLower()).Contains(keyword))
                .ToList();

            if (filtered.Count == 0)
            {
                MessageBox.Show("Không tìm thấy bạn nào phù hợp.", "Thông báo");
            }

            DisplayFriends(filtered);
        }


        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
