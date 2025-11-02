using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace btl_lttq.Friendprofile
{
    public class RoundedTextBox : UserControl
    {
        private readonly TextBox innerTextBox = new TextBox();
        private bool isFocused = false;
        private bool isEditingMode = false;

        public string TextValue
        {
            get => innerTextBox.Text;
            set => innerTextBox.Text = value;
        }

        public bool ReadOnly
        {
            get => innerTextBox.ReadOnly;
            set => innerTextBox.ReadOnly = value;
        }

        public bool EditingMode
        {
            get => isEditingMode;
            set { isEditingMode = value; Invalidate(); }
        }

        public RoundedTextBox()
        {
            // Giao diện chung
            this.BackColor = Color.White;
            this.Padding = new Padding(4);
            this.Size = new Size(180, 28);
            this.ForeColor = Color.Black;
            this.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            innerTextBox.BorderStyle = BorderStyle.None;
            innerTextBox.Font = new Font("Segoe UI", 10);
            innerTextBox.ForeColor = Color.Black;
            innerTextBox.BackColor = Color.White;
            innerTextBox.Dock = DockStyle.Fill;

            // ⚠️ Ngăn tự động chọn text khi form mở
            innerTextBox.GotFocus += (s, e) =>
            {
                if (innerTextBox.ReadOnly)
                {
                    // Nếu đang ở chế độ chỉ xem thì bỏ qua focus
                    this.Parent?.SelectNextControl(this, true, true, true, true);
                    return;
                }

                // Ngăn bôi đen toàn bộ text khi focus
                innerTextBox.SelectionLength = 0;
                isFocused = true;
                Invalidate(); // đổi màu viền
            };

            innerTextBox.LostFocus += (s, e) =>
            {
                isFocused = false;
                Invalidate();
            };

            // ⚙️ Ngăn auto-select text khi nhấn chuột
            innerTextBox.MouseDown += (s, e) =>
            {
                if (innerTextBox.ReadOnly)
                {
                    this.Parent?.SelectNextControl(this, true, true, true, true);
                    e = null;
                }
                else
                {
                    // Dời vị trí con trỏ đúng nơi click, không bôi đen
                    int pos = innerTextBox.GetCharIndexFromPosition(e.Location);
                    innerTextBox.SelectionStart = pos;
                    innerTextBox.SelectionLength = 0;
                }
            };

            // ⚙️ Ngăn việc Windows tự focus textbox đầu tiên trong form
            this.GotFocus += (s, e) => { this.ActiveControl = null; };

            this.Controls.Add(innerTextBox);
        }

        // Vẽ border bo tròn
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color borderColor;
            if (!this.Enabled)
                borderColor = Color.FromArgb(200, 230, 250);
            else if (isFocused)
                borderColor = Color.FromArgb(0, 102, 255); // xanh đậm khi focus
            else
                borderColor = Color.FromArgb(102, 204, 255); // xanh nhạt mặc định

            int radius = 8;
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            using (GraphicsPath path = RoundedRect(rect, radius))
            using (Pen pen = new Pen(borderColor, 2))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public override string Text
        {
            get => innerTextBox.Text;
            set => innerTextBox.Text = value;
        }
    }
}
