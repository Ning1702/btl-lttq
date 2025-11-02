using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace btl_lttq.Login
{
    public partial class ForgotPasswordForm : Form
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["MessengerDb"].ConnectionString;

        public ForgotPasswordForm()
        {
            InitializeComponent();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string userName = txtUserName.Text.Trim();
            string newPwd = txtNewPassword.Text.Trim();

            if (email == "" || userName == "" || newPwd == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Email, Tài khoản và Mật khẩu mới!");
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var checkSql = "SELECT COUNT(*) FROM Users WHERE Email=@Email AND UserName=@UserName";
                    using (var cmd = new SqlCommand(checkSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        int count = (int)cmd.ExecuteScalar();

                        if (count == 0)
                        {
                            MessageBox.Show("Email hoặc tài khoản không đúng!");
                            return;
                        }
                    }

                    var updateSql = @"
                        UPDATE Users
                        SET PasswordHash = CONVERT(VARBINARY(50), @Pwd),
                            PasswordSalt = CONVERT(VARBINARY(50), 'plain')
                        WHERE Email=@Email AND UserName=@UserName";

                    using (var updateCmd = new SqlCommand(updateSql, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Pwd", newPwd);
                        updateCmd.Parameters.AddWithValue("@Email", email);
                        updateCmd.Parameters.AddWithValue("@UserName", userName);
                        updateCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Đặt lại mật khẩu thành công!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message);
            }
        }

        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {

        }
    }
}
