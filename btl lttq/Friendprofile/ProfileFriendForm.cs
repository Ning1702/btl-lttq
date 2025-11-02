using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;                // ✅ Thêm dòng này
using System.Windows.Forms;

namespace btl_lttq.Friendprofile
{
    public partial class ProfileFriendForm : Form
    {
        private Guid friendId;

        public ProfileFriendForm(Guid id)
        {
            InitializeComponent();
            friendId = id;
            LoadFriendProfile();
        }

        private void LoadFriendProfile()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(
                    "Data Source=DESKTOP-G1DJPBN,1433;Initial Catalog=MessengerDb;User ID=sa;Password=123456aA@$;TrustServerCertificate=True;"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetUserProfile", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", friendId);

                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        txtFullName.Text = r["FullName"]?.ToString();
                        txtEmail.Text = r["Email"]?.ToString();
                        txtGender.Text = r["Gender"]?.ToString();
                        txtPhone.Text = r["Phone"]?.ToString();
                        txtHometown.Text = r["Hometown"]?.ToString();
                        txtEducation.Text = r["Education"]?.ToString();
                        txtWork.Text = r["Work"]?.ToString();
                        txtRelationship.Text = r["Relationship"]?.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin người dùng này!", "Thông báo");
                    }
                    r.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải thông tin bạn bè: " + ex.Message);
            }
        }

        private void ProfileFriendForm_Load(object sender, EventArgs e)
        {
            Color bgColor = Color.FromArgb(245, 245, 245); // nền xám nhạt
            Color textColor = Color.Black;                 // chữ đen rõ ràng

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is btl_lttq.Friendprofile.RoundedTextBox rtb)
                {
                    var innerText = rtb.Controls.OfType<TextBox>().FirstOrDefault();
                    if (innerText != null)
                    {
                        innerText.ReadOnly = true;
                        innerText.TabStop = false;
                        innerText.BackColor = bgColor;
                        innerText.ForeColor = textColor;
                        innerText.BorderStyle = BorderStyle.None;
                        innerText.Cursor = Cursors.Default;

                        // ❌ Chặn focus và nhập liệu
                        innerText.GotFocus += (s, e2) => this.ActiveControl = null;
                        innerText.MouseDown += (s, e2) => this.ActiveControl = null;
                        innerText.KeyPress += (s, e2) => e2.Handled = true; // ngăn gõ phím
                    }

                    rtb.TabStop = false;
                    rtb.BackColor = this.BackColor;
                }
                else if (ctrl is TextBox txt)
                {
                    txt.ReadOnly = true;
                    txt.TabStop = false;
                    txt.BackColor = bgColor;
                    txt.ForeColor = textColor;
                    txt.BorderStyle = BorderStyle.None;
                    txt.Cursor = Cursors.Default;

                    txt.GotFocus += (s, e2) => this.ActiveControl = null;
                    txt.MouseDown += (s, e2) => this.ActiveControl = null;
                    txt.KeyPress += (s, e2) => e2.Handled = true;
                }
                else if (ctrl is ComboBox cb)
                {
                    cb.Enabled = false;
                    cb.BackColor = bgColor;
                    cb.ForeColor = textColor;
                    cb.Cursor = Cursors.Default;
                }
            }

            // 🔹 Không auto-focus khi mở form
            this.ActiveControl = null;
            this.BeginInvoke(new Action(() => this.ActiveControl = null));
        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.ActiveControl = null;  // bỏ focus ban đầu
            this.BeginInvoke(new Action(() => this.ActiveControl = null)); // bỏ focus lần 2 sau khi form render
        }
    }
}
