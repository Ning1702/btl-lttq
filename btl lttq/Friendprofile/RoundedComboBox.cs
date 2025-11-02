using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace btl_lttq.Friendprofile
{
    public class RoundedComboBox : ComboBox
    {
        private bool _isFocused = false;

        public const int DefaultHeight = 34;   // chiều cao cố định cho TẤT CẢ form
        private const int BorderRadius = 10;
        private const int BorderThickness = 2;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        public RoundedComboBox()
        {
            // Vẽ tay & đồng bộ item
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            FlatStyle = FlatStyle.Flat;
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;

            Font = new Font("Segoe UI", 10f);
            BackColor = Color.White;
            ForeColor = Color.FromArgb(30, 30, 30);

            // Quan trọng: cho phép set chiều cao custom
            IntegralHeight = false;
            ItemHeight = 28;
            DropDownHeight = ItemHeight * 6;

            // Lock chiều cao ban đầu
            Height = DefaultHeight;
            MinimumSize = new Size(0, DefaultHeight);

            GotFocus += (s, e) => { _isFocused = true; Invalidate(); };
            LostFocus += (s, e) => { _isFocused = false; Invalidate(); };
        }

        // Khóa chiều cao khi form/parent Resize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (Height != DefaultHeight) Height = DefaultHeight;
        }

        // Khi đổi font (form này có thể khác form kia) vẫn giữ Height
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (Height != DefaultHeight) Height = DefaultHeight;
        }

        // Khi tạo control (ở cả runtime & designer) thiết lập lại các thông số kích thước
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // Ngăn Designer/AutoScale “làm cao lên”
            Height = DefaultHeight;
            MinimumSize = new Size(0, DefaultHeight);
            IntegralHeight = false;
            ItemHeight = 28;
            DropDownHeight = ItemHeight * 6;

            // Preview trong Design mode
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                if (Items.Count == 0)
                    Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            }
        }

        // Vẽ nền + viền + text
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var bg = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(bg, ClientRectangle);

            // Text khi dropdown đóng
            string displayText = (SelectedIndex >= 0) ? GetItemText(SelectedItem) : string.Empty;
            TextRenderer.DrawText(
                e.Graphics,
                displayText,
                Font,
                new Rectangle(8, 4, Width - 26, Height - 8),
                ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            // Nút mũi tên
            var arrowRect = new Rectangle(Width - 22, 0, 22, Height);
            TextRenderer.DrawText(e.Graphics, "▾", Font, arrowRect, ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);

            // Viền bo tròn
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = GetRoundPath(rect, BorderRadius))
            using (Pen pen = new Pen(_isFocused ? Color.FromArgb(0, 102, 255)
                                                : Color.FromArgb(102, 204, 255), BorderThickness))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0 && e.Index < Items.Count)
            {
                string text = GetItemText(Items[e.Index]);
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                using (var back = new SolidBrush(selected ? Color.FromArgb(0, 102, 255) : BackColor))
                using (var fore = new SolidBrush(selected ? Color.White : ForeColor))
                {
                    e.Graphics.FillRectangle(back, e.Bounds);
                    e.Graphics.DrawString(text, Font, fore, e.Bounds.X + 8, e.Bounds.Y + 4);
                }
            }
            e.DrawFocusRectangle();
        }

        private static GraphicsPath GetRoundPath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius, radius, 180, 90);
            path.AddArc(r.Right - radius, r.Y, radius, radius, 270, 90);
            path.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(r.X, r.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
