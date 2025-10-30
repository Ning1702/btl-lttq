using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace btl_lttq
{
    public class RoundedTextBox : UserControl
    {
        private TextBox innerTextBox = new TextBox();
        private bool isFocused = false;

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

        public RoundedTextBox()
        {
            this.BackColor = Color.White;
            this.Padding = new Padding(4);
            this.Size = new Size(180, 28);

            innerTextBox.BorderStyle = BorderStyle.None;
            innerTextBox.Font = new Font("Segoe UI", 10);
            innerTextBox.ForeColor = Color.DimGray;
            innerTextBox.BackColor = Color.White;
            innerTextBox.Dock = DockStyle.Fill;

            innerTextBox.GotFocus += (s, e) => { isFocused = true; Invalidate(); };
            innerTextBox.LostFocus += (s, e) => { isFocused = false; Invalidate(); };

            this.Controls.Add(innerTextBox);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color borderColor = isFocused
                ? Color.FromArgb(51, 204, 255)   // #33CCFF khi focus
                : Color.FromArgb(102, 204, 255); // #66CCFF mặc định

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

        // Đồng bộ giá trị Text cho code cũ
        public override string Text
        {
            get => innerTextBox.Text;
            set => innerTextBox.Text = value;
        }
    }
}
