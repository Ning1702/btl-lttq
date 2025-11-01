using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace btl_lttq.Friendprofile
{
    public class RoundedTextBox : UserControl
    {
        private TextBox innerTextBox = new TextBox();
        private bool isFocused = false;
        private bool isEditingMode = false; // chế độ sửa thông tin

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
            // Giao diện
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

            // Focus
            innerTextBox.GotFocus += (s, e) =>
            {
                if (innerTextBox.ReadOnly)
                {
                    this.Parent?.SelectNextControl(this, true, true, true, true);
                    return;
                }
                isFocused = true;
                Invalidate(); // vẽ lại viền xanh đậm khi focus
            };

            innerTextBox.LostFocus += (s, e) =>
            {
                isFocused = false;
                Invalidate(); // trở lại xanh nhạt khi mất focus
            };

            // Chặn click khi bị khóa
            this.MouseDown += (s, e) =>
            {
                if (innerTextBox.ReadOnly)
                    this.Parent?.SelectNextControl(this, true, true, true, true);
            };
            innerTextBox.MouseDown += (s, e) =>
            {
                if (innerTextBox.ReadOnly)
                    this.Parent?.SelectNextControl(this, true, true, true, true);
            };

            this.Controls.Add(innerTextBox);
        }

        // Vẽ border bo tròn
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 🔹 Quy tắc chọn màu viền:
            // - Không focus → xanh nhạt (#66CCFF)
            // - Focus → xanh đậm (#0066FF)
            // - Bị khóa → xám nhạt
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
