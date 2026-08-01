using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ImageEditor
{
    /// <summary>
    /// Petits outils graphiques partagés par les contrôles modernes.
    /// Ils restent volontairement simples afin que le code soit facile à modifier.
    /// </summary>
    internal static class UiDrawing
    {
        public static GraphicsPath CreateRoundedRectangle(Rectangle rectangle, int radius)
        {
            var path = new GraphicsPath();

            if (rectangle.Width <= 0 || rectangle.Height <= 0)
                return path;

            int diameter = Math.Min(radius * 2, Math.Min(rectangle.Width, rectangle.Height));
            if (diameter <= 1)
            {
                path.AddRectangle(rectangle);
                return path;
            }

            var arc = new Rectangle(rectangle.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);
            arc.X = rectangle.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }
    }

    /// <summary>
    /// Panel avec des coins arrondis et une bordure légère.
    /// Il est utilisé comme une « carte » pour regrouper les réglages.
    /// </summary>
    public class RoundedPanel : Panel
    {
        private int _cornerRadius = 16;
        private int _borderSize = 1;
        private Color _borderColor = Color.FromArgb(226, 232, 240);

        [Category("Apparence")]
        [DefaultValue(16)]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = Math.Max(0, value);
                UpdateRoundedRegion();
                Invalidate();
            }
        }

        [Category("Apparence")]
        [DefaultValue(1)]
        public int BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Apparence")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        public RoundedPanel()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            BackColor = Color.White;
        }

        protected override void OnResize(EventArgs eventArgs)
        {
            base.OnResize(eventArgs);
            UpdateRoundedRegion();
        }

        private void UpdateRoundedRegion()
        {
            if (Width <= 0 || Height <= 0)
                return;

            var rectangle = new Rectangle(0, 0, Width, Height);
            using (GraphicsPath path = UiDrawing.CreateRoundedRectangle(rectangle, _cornerRadius))
            {
                Region previousRegion = Region;
                Region = new Region(path);

                if (previousRegion != null)
                    previousRegion.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rectangle = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = UiDrawing.CreateRoundedRectangle(rectangle, _cornerRadius))
            using (var backgroundBrush = new SolidBrush(BackColor))
            {
                eventArgs.Graphics.FillPath(backgroundBrush, path);

                if (_borderSize > 0)
                {
                    using (var borderPen = new Pen(_borderColor, _borderSize))
                        eventArgs.Graphics.DrawPath(borderPen, path);
                }
            }

            base.OnPaint(eventArgs);
        }
    }

    /// <summary>
    /// Bouton dessiné manuellement pour avoir des coins arrondis
    /// et des couleurs différentes au survol et pendant le clic.
    /// </summary>
    public class ModernButton : Button
    {
        private bool _mouseOver;
        private bool _mouseDown;
        private int _cornerRadius = 12;
        private Color _hoverBackColor = Color.FromArgb(29, 78, 216);
        private Color _pressedBackColor = Color.FromArgb(30, 64, 175);
        private Color _disabledBackColor = Color.FromArgb(203, 213, 225);
        private Color _disabledTextColor = Color.FromArgb(100, 116, 139);

        [Category("Apparence")]
        [DefaultValue(12)]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Apparence")]
        public Color HoverBackColor
        {
            get { return _hoverBackColor; }
            set { _hoverBackColor = value; }
        }

        [Category("Apparence")]
        public Color PressedBackColor
        {
            get { return _pressedBackColor; }
            set { _pressedBackColor = value; }
        }

        [Category("Apparence")]
        public Color DisabledBackColor
        {
            get { return _disabledBackColor; }
            set { _disabledBackColor = value; }
        }

        [Category("Apparence")]
        public Color DisabledTextColor
        {
            get { return _disabledTextColor; }
            set { _disabledTextColor = value; }
        }

        public ModernButton()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            BackColor = Color.FromArgb(37, 99, 235);
            ForeColor = Color.White;
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        }

        protected override void OnMouseEnter(EventArgs eventArgs)
        {
            _mouseOver = true;
            Invalidate();
            base.OnMouseEnter(eventArgs);
        }

        protected override void OnMouseLeave(EventArgs eventArgs)
        {
            _mouseOver = false;
            _mouseDown = false;
            Invalidate();
            base.OnMouseLeave(eventArgs);
        }

        protected override void OnMouseDown(MouseEventArgs eventArgs)
        {
            if (eventArgs.Button == MouseButtons.Left)
            {
                _mouseDown = true;
                Invalidate();
            }

            base.OnMouseDown(eventArgs);
        }

        protected override void OnMouseUp(MouseEventArgs eventArgs)
        {
            _mouseDown = false;
            Invalidate();
            base.OnMouseUp(eventArgs);
        }

        protected override void OnEnabledChanged(EventArgs eventArgs)
        {
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
            base.OnEnabledChanged(eventArgs);
        }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color backgroundColor;
            Color textColor;

            if (!Enabled)
            {
                backgroundColor = _disabledBackColor;
                textColor = _disabledTextColor;
            }
            else
            {
                backgroundColor = _mouseDown
                    ? _pressedBackColor
                    : (_mouseOver ? _hoverBackColor : BackColor);
                textColor = ForeColor;
            }

            var rectangle = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = UiDrawing.CreateRoundedRectangle(rectangle, _cornerRadius))
            using (var backgroundBrush = new SolidBrush(backgroundColor))
            {
                eventArgs.Graphics.FillPath(backgroundBrush, path);
            }

            TextRenderer.DrawText(
                eventArgs.Graphics,
                Text,
                Font,
                ClientRectangle,
                textColor,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis);

            if (Focused && ShowFocusCues)
            {
                var focusRectangle = ClientRectangle;
                focusRectangle.Inflate(-5, -5);
                ControlPaint.DrawFocusRectangle(eventArgs.Graphics, focusRectangle, textColor, backgroundColor);
            }
        }
    }

    /// <summary>
    /// PictureBox avec un quadrillage discret derrière l'image.
    /// Le quadrillage permet de voir immédiatement les zones transparentes.
    /// Quand aucune image n'est chargée, un petit message est dessiné au centre.
    /// </summary>
    public class PreviewPictureBox : PictureBox
    {
        // Ces polices sont créées une seule fois puis libérées avec le contrôle.
        // Cela évite de recréer des objets graphiques à chaque redessin de la fenêtre.
        private readonly Font _emptyTitleFont = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        private readonly Font _emptySubtitleFont = new Font("Segoe UI", 9F);

        public PreviewPictureBox()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            SizeMode = PictureBoxSizeMode.Zoom;
            BackColor = Color.FromArgb(15, 23, 42);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _emptyTitleFont.Dispose();
                _emptySubtitleFont.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnPaintBackground(PaintEventArgs eventArgs)
        {
            const int squareSize = 18;
            Color firstColor = Color.FromArgb(24, 34, 52);
            Color secondColor = Color.FromArgb(31, 43, 64);

            using (var firstBrush = new SolidBrush(firstColor))
            using (var secondBrush = new SolidBrush(secondColor))
            {
                eventArgs.Graphics.FillRectangle(firstBrush, ClientRectangle);

                for (int y = 0; y < Height; y += squareSize)
                {
                    for (int x = 0; x < Width; x += squareSize)
                    {
                        bool useSecondColor = ((x / squareSize) + (y / squareSize)) % 2 == 0;
                        if (useSecondColor)
                            eventArgs.Graphics.FillRectangle(secondBrush, x, y, squareSize, squareSize);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            base.OnPaint(eventArgs);

            if (Image != null)
                return;

            eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int iconWidth = 74;
            int iconHeight = 56;
            int iconX = (Width - iconWidth) / 2;
            int iconY = Math.Max(24, (Height - iconHeight) / 2 - 42);
            var iconRectangle = new Rectangle(iconX, iconY, iconWidth, iconHeight);

            using (var iconPen = new Pen(Color.FromArgb(148, 163, 184), 2F))
            {
                eventArgs.Graphics.DrawRectangle(iconPen, iconRectangle);
                eventArgs.Graphics.DrawEllipse(iconPen, iconX + 49, iconY + 10, 11, 11);

                Point[] mountainPoints =
                {
                    new Point(iconX + 8, iconY + 45),
                    new Point(iconX + 28, iconY + 25),
                    new Point(iconX + 40, iconY + 37),
                    new Point(iconX + 50, iconY + 29),
                    new Point(iconX + 66, iconY + 45)
                };
                eventArgs.Graphics.DrawLines(iconPen, mountainPoints);
            }

            var titleRectangle = new Rectangle(20, iconY + iconHeight + 20, Width - 40, 28);
            TextRenderer.DrawText(
                eventArgs.Graphics,
                "Dépose une image ici",
                _emptyTitleFont,
                titleRectangle,
                Color.FromArgb(226, 232, 240),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            var subtitleRectangle = new Rectangle(20, iconY + iconHeight + 48, Width - 40, 24);
            TextRenderer.DrawText(
                eventArgs.Graphics,
                "ou utilise le bouton « Ouvrir une image »",
                _emptySubtitleFont,
                subtitleRectangle,
                Color.FromArgb(148, 163, 184),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
