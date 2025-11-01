using btl_lttq.ChatClient;
using btl_lttq.Login.Services;
using System;
using System.Windows.Forms;

namespace btl_lttq.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                if (AuthService.Login(email, password))
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    this.Hide();
                    using (var messengerForm = new MessengerForm())
                    {
                        messengerForm.ShowDialog();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai email hoặc mật khẩu!");
                    // không đóng form, cho user thử lại
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }


        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new RegisterForm().ShowDialog();
        }

        private void linkForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new ForgotPasswordForm().ShowDialog();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
