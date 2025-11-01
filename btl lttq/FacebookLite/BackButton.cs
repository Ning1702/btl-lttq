using System;
using System.Drawing;
using System.Windows.Forms;

namespace btl_lttq.FacebookLite
{
    public class BackButton : Control
    {
        public BackButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Size = new Size(32, 32);
            Cursor = Cursors.Hand;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            using var pen = new System.Drawing.Pen(this.ForeColor.IsEmpty ? Color.Gray : this.ForeColor, 2f);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int cx = Width / 2, cy = Height / 2;
            g.DrawLines(pen, new[] { new Point(cx+6, cy-8), new Point(cx-6, cy), new Point(cx+6, cy+8) });
        }
    }
}
