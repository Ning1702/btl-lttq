using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace btl_lttq
{
    public class RoundedComboBox : ComboBox
    {
        private bool _isFocused = false;

        public RoundedComboBox()
        {
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.White;
            ForeColor = Color.DimGray;
            Font = new Font("Segoe UI", 10);
            DropDownStyle = ComboBoxStyle.DropDownList;
            SetStyle(ControlStyles.UserPaint, true);

            this.GotFocus += (s, e) => { _isFocused = true; Invalidate(); };
            this.LostFocus += (s, e) => { _isFocused = false; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 8;
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Color borderColor = _isFocused
                    ? Color.FromArgb(51, 204, 255)   // #33CCFF khi focus
                    : Color.FromArgb(102, 204, 255); // #66CCFF bình thường

                using (Pen pen = new Pen(borderColor, 2))
                    e.Graphics.DrawPath(pen, path);
            }
        }
    }
}
