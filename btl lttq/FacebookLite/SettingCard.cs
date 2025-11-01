using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace btl_lttq.FacebookLite
{
    public partial class SettingCard : UserControl
    {
        private Color _baseColor = Color.FromArgb(248, 249, 250);
        private Color _borderColor = Color.FromArgb(210, 213, 218);
        private int _borderThickness = 1;
        private int _cornerRadius = 18;

        private float _hoverLighten = 0.10f;   // 10%
        private bool _hoverDarken = false;     // Light theme => darken, Dark theme => lighten
        private bool _hoverEnabled = true;
        private bool _hovered = false;
        private bool _pressLatch = false;

        private bool _isClickable = false;

        [Category("Behavior")]
        public bool FastClick { get; set; } = true;  // click ngay tại MouseDown với pill

        public SettingCard()
        {
            InitializeComponent();

            // Card không tranh focus của Button/ComboBox bên trong
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Padding = new Padding(12);
            UpdateRegion();
        }

        [Category("Appearance")] public Color BaseColor { get => _baseColor; set { _baseColor = value; Invalidate(); } }
        [Category("Appearance")] public Color BorderColor { get => _borderColor; set { _borderColor = value; Invalidate(); } }
        [Category("Appearance")] public int BorderThickness { get => _borderThickness; set { _borderThickness = Math.Max(0, value); Invalidate(); } }
        [Category("Appearance")] public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(0, value); Invalidate(); UpdateRegion(); } }

        [Category("Behavior")] public float HoverLighten { get => _hoverLighten; set { _hoverLighten = Math.Max(0f, Math.Min(1f, value)); Invalidate(); } }
        [Category("Behavior")] public bool HoverDarken  { get => _hoverDarken;  set { _hoverDarken = value; Invalidate(); } }
        [Category("Behavior")] public bool HoverEnabled { get => _hoverEnabled; set { _hoverEnabled = value; if (!value) { _hovered = false; _pressLatch = false; } Invalidate(); } }
        [Category("Behavior")] public bool IsClickable  { get => _isClickable;  set { _isClickable = value; Cursor = value ? Cursors.Hand : Cursors.Default; } }

        // ==== Interactive controls exclusion ====
        private static bool IsInteractiveControl(Control c)
        {
            return c is ButtonBase || c is Button || c is LinkLabel ||
                   c is ComboBox || c is ListBox || c is CheckedListBox ||
                   c is TextBoxBase || c is NumericUpDown || c is DateTimePicker ||
                   c is CheckBox || c is RadioButton || c is TrackBar ||
                   c is VScrollBar || c is HScrollBar;
        }

        private bool InteractiveAt(Point p)
        {
            Control hit = GetChildAtPoint(p, GetChildAtPointSkip.Invisible | GetChildAtPointSkip.Disabled);
            while (hit != null && hit != this)
            {
                if (IsInteractiveControl(hit)) return true;
                hit = hit.Parent;
            }
            return false;
        }

        private bool CursorOverInteractiveChild()
        {
            return InteractiveAt(PointToClient(Cursor.Position));
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            WireChildInput(this);
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            WireChildInput(e.Control);
        }

        private void WireChildInput(Control root)
        {
            foreach (Control c in root.Controls)
            {
                if (IsInteractiveControl(c))
                {
                    c.MouseEnter += (s, e) => { HoverEnabled = false; Invalidate(); };
                    c.MouseLeave += (s, e) => { HoverEnabled = true;  Invalidate(); };

                    if (c is ComboBox cb)
                    {
                        cb.DropDown += (s, e) => { HoverEnabled = false; };
                        cb.DropDownClosed += (s, e) => { HoverEnabled = true; };
                    }
                    if (c is ButtonBase)
                    {
                        c.MouseDown += (s, e) => { _pressLatch = false; Capture = false; };
                        c.MouseUp   += (s, e) => { _pressLatch = false; Capture = false; };
                    }
                }
                else
                {
                    c.MouseEnter += (s, e) => { if (HoverEnabled) SetHover(true); };
                    c.MouseLeave += (s, e) =>
                    {
                        if (!_pressLatch)
                        {
                            bool insideNow = ClientRectangle.Contains(PointToClient(Cursor.Position));
                            SetHover(insideNow);
                        }
                    };
                    c.MouseDown += (s, e) =>
                    {
                        if (!HoverEnabled) return;
                        _pressLatch = true; Capture = true; Invalidate();

                        if (FastClick && e.Button == MouseButtons.Left)
                            OnClick(EventArgs.Empty);
                    };
                    c.MouseUp   += (s, e) =>
                    {
                        if (!HoverEnabled) return;
                        _pressLatch = false; Capture = false;
                        bool insideNow = ClientRectangle.Contains(PointToClient(Cursor.Position));
                        SetHover(insideNow);
                    };
                }

                if (c.HasChildren) WireChildInput(c);
            }
        }

        // ===== Mouse on card =====
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!HoverEnabled) return;
            if (InteractiveAt(PointToClient(Cursor.Position)) || CursorOverInteractiveChild()) return;
            SetHover(true);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!_pressLatch) SetHover(false);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!HoverEnabled || InteractiveAt(e.Location) || CursorOverInteractiveChild()) return;

            _pressLatch = true;
            Capture = true;
            Invalidate();

            if (FastClick && e.Button == MouseButtons.Left)
                OnClick(EventArgs.Empty);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            bool insideNow = ClientRectangle.Contains(e.Location);
            _pressLatch = false; Capture = false;
            if (!HoverEnabled || InteractiveAt(e.Location) || CursorOverInteractiveChild()) { SetHover(false); return; }
            SetHover(insideNow);
        }

        private void SetHover(bool on)
        {
            if (!HoverEnabled)
            {
                if (_hovered) { _hovered = false; Invalidate(); }
                return;
            }
            if (_hovered == on) return;
            _hovered = on;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            using (GraphicsPath path = CreateRoundRectPath(new Rectangle(0, 0, Width - 1, Height - 1), _cornerRadius))
            {
                Region = new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            RectangleF rect = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            float inset = Math.Max(0, _borderThickness);
            RectangleF fillRect = RectangleF.Inflate(rect, -inset, -inset);

            using (GraphicsPath borderPath = CreateRoundRectPathF(rect, _cornerRadius))
            using (GraphicsPath fillPath   = CreateRoundRectPathF(fillRect, Math.Max(0, _cornerRadius - (int)inset)))
            using (SolidBrush br = new SolidBrush(GetFillColor()))
            {
                g.FillPath(br, fillPath);
                if (_borderThickness > 0)
                {
                    using (Pen pen = new Pen(_borderColor, _borderThickness) { Alignment = PenAlignment.Inset })
                    {
                        g.DrawPath(pen, borderPath);
                    }
                }
            }
        }

        private Color GetFillColor()
        {
            if (!HoverEnabled || CursorOverInteractiveChild()) return _baseColor;
            if ((!_hovered && !_pressLatch) || _hoverLighten <= 0f) return _baseColor;
            return _hoverDarken ? Darken(_baseColor, _hoverLighten) : Lighten(_baseColor, _hoverLighten);
        }

        private static GraphicsPath CreateRoundRectPath(Rectangle bounds, int radius)
        {
            int d = Math.Max(0, radius * 2);
            int right = bounds.Right;
            int bottom = bounds.Bottom;

            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(right - d, bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateRoundRectPathF(RectangleF bounds, int radius)
        {
            float d = Math.Max(0, radius * 2);
            float right = bounds.Right;
            float bottom = bounds.Bottom;

            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(right - d, bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Color Lighten(Color c, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            int r = c.R + (int)((255 - c.R) * t);
            int g = c.G + (int)((255 - c.G) * t);
            int b = c.B + (int)((255 - c.B) * t);
            return Color.FromArgb(c.A, r, g, b);
        }
        private static Color Darken(Color c, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            int r = (int)(c.R * (1f - t));
            int g = (int)(c.G * (1f - t));
            int b = (int)(c.B * (1f - t));
            return Color.FromArgb(c.A, r, g, b);
        }
    }
}
