using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace btl_lttq
{
    public partial class ProfileForm : Form
    {
        private string CurrentUsername = "anninh";
        private bool isEditing = false;

        public ProfileForm()
        {
            InitializeComponent();
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            LoadUserProfile();
            SetEditMode(false);

            this.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.BackColor = Color.WhiteSmoke;

            StyleButton(btnEdit, Color.FromArgb(66, 133, 244));   // xanh biển
            StyleButton(btnUpdate, Color.FromArgb(52, 168, 83));  // xanh lá
        }

        private void StyleButton(Button btn, Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 12, 12));

            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(color, 0.2f);
            btn.MouseLeave += (s, e) => btn.BackColor = color;
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void LoadUserProfile()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(
                    "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
                {
                    conn.Open();
                    string sql = @"
                        SELECT FullName, Email, PhoneNumber, Gender, Hometown, Education, Work, Relationship, AvatarUrl
                        FROM Users WHERE UserName = @u";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", CurrentUsername);

                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        txtFullName.Text = r["FullName"].ToString();
                        txtEmail.Text = r["Email"].ToString();
                        txtPhone.Text = r["PhoneNumber"].ToString();
                        cboGender.Text = r["Gender"].ToString();
                        txtHometown.Text = r["Hometown"].ToString();
                        txtEducation.Text = r["Education"].ToString();
                        txtWork.Text = r["Work"].ToString();
                        txtRelationship.Text = r["Relationship"].ToString();

                        string avatarPath = Path.Combine(Application.StartupPath, "Images", r["AvatarUrl"].ToString());
                        if (File.Exists(avatarPath))
                            picAvatar.Image = Image.FromFile(avatarPath);
                        else
                            picAvatar.BackColor = Color.LightGray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải hồ sơ: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isEditing = !isEditing;
            SetEditMode(isEditing);
            btnEdit.Text = isEditing ? "🔒 Hủy sửa" : "✏️ Sửa thông tin";
        }

        private void SetEditMode(bool enable)
        {
            txtFullName.ReadOnly = !enable;
            txtPhone.ReadOnly = !enable;
            cboGender.Enabled = enable;
            txtHometown.ReadOnly = !enable;
            txtEducation.ReadOnly = !enable;
            txtWork.ReadOnly = !enable;
            txtRelationship.ReadOnly = !enable;

            btnUpdate.Enabled = enable;
            picAvatar.Enabled = enable;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Hãy nhấn 'Sửa thông tin' trước khi cập nhật!", "Thông báo");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(
                    "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
                {
                    conn.Open();
                    string sql = @"
                        UPDATE Users SET
                            FullName = @n,
                            PhoneNumber = @p,
                            Gender = @g,
                            Hometown = @h,
                            Education = @e,
                            Work = @w,
                            Relationship = @r,
                            UpdatedAt = SYSUTCDATETIME()
                        WHERE UserName = @u";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@n", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@g", cboGender.Text);
                    cmd.Parameters.AddWithValue("@h", txtHometown.Text);
                    cmd.Parameters.AddWithValue("@e", txtEducation.Text);
                    cmd.Parameters.AddWithValue("@w", txtWork.Text);
                    cmd.Parameters.AddWithValue("@r", txtRelationship.Text);
                    cmd.Parameters.AddWithValue("@u", CurrentUsername);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Cập nhật hồ sơ thành công!");
                SetEditMode(false);
                btnEdit.Text = "✏️ Sửa thông tin";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi cập nhật: " + ex.Message);
            }
        }

        private void picAvatar_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Hãy bật chế độ chỉnh sửa để thay ảnh!", "Thông báo");
                return;
            }

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Ảnh (*.jpg;*.png)|*.jpg;*.png";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileName(dlg.FileName);
                string destPath = Path.Combine(Application.StartupPath, "Images", fileName);
                File.Copy(dlg.FileName, destPath, true);

                picAvatar.Image = Image.FromFile(destPath);

                using (SqlConnection conn = new SqlConnection(
                    "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Users SET AvatarUrl = @a WHERE UserName = @u", conn);
                    cmd.Parameters.AddWithValue("@a", fileName);
                    cmd.Parameters.AddWithValue("@u", CurrentUsername);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
