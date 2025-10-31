using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace btl_lttq
{
    public class RoundedComboBox : ComboBox
    {
        private bool _isFocused = false;
        private const int BorderRadius = 10;
        private const int BorderThickness = 2;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        public RoundedComboBox()
        {
            
            // 🔹 Tắt viền mặc định và bật chế độ vẽ tay
            SetStyle(ControlStyles.UserPaint, true);
            FlatStyle = FlatStyle.Flat;

            // 🔹 Màu sắc và font
            BackColor = Color.White;
            ForeColor = Color.Black;
            Font = new Font("Segoe UI", 10);
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 28;

            // 🔹 Sự kiện focus để đổi màu viền
            this.GotFocus += (s, e) => { _isFocused = true; Invalidate(); };
            this.LostFocus += (s, e) => { _isFocused = false; Invalidate(); };
        }

        // ✅ Sửa lỗi “tàng hình” bằng cách vẽ nền và border
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 🔹 Vẽ nền trắng cho ComboBox (tránh bị trong suốt)
            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            // 🔹 Hiển thị text (khi dropdown đóng)
            string displayText = (SelectedIndex >= 0) ? GetItemText(SelectedItem) : string.Empty;
            TextRenderer.DrawText(
                e.Graphics,
                displayText,
                Font,
                new Rectangle(8, 4, Width - 25, Height - 8),
                ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            // 🔹 Bo tròn khung viền
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            int radius = BorderRadius;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                Color borderColor = _isFocused ? Color.FromArgb(0, 102, 255) : Color.FromArgb(102, 204, 255);
                using (Pen pen = new Pen(borderColor, BorderThickness))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0 && e.Index < Items.Count)
            {
                string text = GetItemText(Items[e.Index]);
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                Color backColor = selected ? Color.FromArgb(0, 102, 255) : BackColor;
                Color textColor = selected ? Color.White : ForeColor;

                using (SolidBrush brush = new SolidBrush(backColor))
                    e.Graphics.FillRectangle(brush, e.Bounds);

                TextRenderer.DrawText(
                    e.Graphics,
                    text,
                    Font,
                    e.Bounds,
                    textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

                e.DrawFocusRectangle();
            }
        }

        // 🔹 Hiển thị mẫu trong Design mode
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                Items.Clear();
                Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            }
        }
    }
}
