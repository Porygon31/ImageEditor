using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageEditor
{
    public partial class MainForm : Form
    {
        private readonly ImageProcessor _proc = new ImageProcessor();
        private readonly Timer _debounce = new Timer { Interval = 200 };
        private readonly ToolTip _tips = new ToolTip { AutoPopDelay = 15000, InitialDelay = 400, ReshowDelay = 100 };

        private Bitmap _originalBmp;   // original, for manual-crop display
        private Bitmap _previewBmp;    // generated result preview (owned)
        private Size _origSize;
        private double _aspect = 1.0;
        private bool _suspend;          // guards programmatic control updates

        // Manual crop selection, in ORIGINAL image pixel coordinates.
        private Rectangle _manualRectImg;
        private bool _dragging;
        private Point _dragStartImg;
        private Rectangle _rectAtDragStart;
        private DragMode _dragMode = DragMode.None;

        private const int HandlePx = 8;   // handle square size (display px)
        private const int HitPx = 7;      // hit tolerance around edges (display px)
        private const int MinCrop = 4;    // minimum selection size (image px)

        private enum DragMode { None, New, Move, N, S, E, W, NE, NW, SE, SW }

        public MainForm()
        {
            InitializeComponent();

            cmbFormat.SelectedIndex = 1; // JPEG

            btnOpen.Click += (s, e) => OpenViaDialog();
            btnSave.Click += (s, e) => SaveResult();

            numWidth.ValueChanged += (s, e) => OnWidthChanged();
            numHeight.ValueChanged += (s, e) => OnHeightChanged();
            chkKeepRatio.CheckedChanged += (s, e) => ScheduleUpdate();

            rbNone.CheckedChanged += (s, e) => OnCropModeChanged();
            rbCover.CheckedChanged += (s, e) => OnCropModeChanged();
            rbContain.CheckedChanged += (s, e) => OnCropModeChanged();
            rbManual.CheckedChanged += (s, e) => OnCropModeChanged();

            cmbFormat.SelectedIndexChanged += (s, e) => { UpdateCompressionEnabled(); ScheduleUpdate(); };
            trkQuality.ValueChanged += (s, e) => { lblQuality.Text = "Qualité : " + trkQuality.Value; ScheduleUpdate(); };
            chkTargetSize.CheckedChanged += (s, e) => { UpdateCompressionEnabled(); ScheduleUpdate(); };
            numTargetKb.ValueChanged += (s, e) => ScheduleUpdate();

            picPreview.MouseDown += PicPreview_MouseDown;
            picPreview.MouseMove += PicPreview_MouseMove;
            picPreview.MouseUp += PicPreview_MouseUp;
            picPreview.Paint += PicPreview_Paint;
            picPreview.Resize += (s, e) => picPreview.Invalidate();

            this.DragEnter += MainForm_DragEnter;
            this.DragDrop += MainForm_DragDrop;
            this.FormClosed += (s, e) => CleanUp();

            _debounce.Tick += (s, e) => { _debounce.Stop(); DoUpdate(); };

            SetupToolTips();
            UpdateCompressionEnabled();
        }

        private void SetupToolTips()
        {
            _tips.SetToolTip(btnOpen, "Ouvrir une image (ou glissez-déposez un fichier sur la fenêtre).");
            _tips.SetToolTip(numWidth, "Largeur de sortie, en pixels.");
            _tips.SetToolTip(numHeight, "Hauteur de sortie, en pixels.");
            _tips.SetToolTip(chkKeepRatio,
                "Conserve les proportions d'origine : changer la largeur ajuste la hauteur.\nActif uniquement en recadrage « Aucun ».");
            _tips.SetToolTip(rbNone,
                "Aucun recadrage : l'image est redimensionnée pour tenir dans la taille\n(proportions conservées si « Garder le ratio » est coché).");
            _tips.SetToolTip(rbCover,
                "Cover : remplit toute la zone cible puis rogne le débord (centré).\nIdéal pour un avatar carré.");
            _tips.SetToolTip(rbContain,
                "Contain : l'image entière tient dans la zone, avec des marges\n(transparentes, ou blanches en JPEG).");
            _tips.SetToolTip(rbManual,
                "Manuel : cliquez-glissez pour dessiner la zone, puis déplacez les bords/coins pour l'ajuster.");
            _tips.SetToolTip(cmbFormat,
                "Format du fichier de sortie.\nWebP = plus léger · PNG = sans perte + transparence · JPEG = photos · GIF.");
            _tips.SetToolTip(trkQuality,
                "Qualité de compression (JPEG/WebP).\nPlus élevé = meilleure image mais fichier plus lourd.");
            _tips.SetToolTip(chkTargetSize,
                "Vise un poids maximum : la qualité est baissée automatiquement\npour passer sous la cible (JPEG/WebP).");
            _tips.SetToolTip(numTargetKb, "Poids maximum visé, en Ko.");
            _tips.SetToolTip(btnSave, "Enregistrer l'image traitée sur le disque.");
            _tips.SetToolTip(picPreview,
                "En recadrage manuel : cliquez-glissez pour dessiner la zone,\npuis déplacez les bords/coins pour l'ajuster.");
        }

        // ---- Loading -------------------------------------------------------

        private void OpenViaDialog()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.webp;*.tiff;*.tif|Tous les fichiers|*.*";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    LoadImage(dlg.FileName);
            }
        }

        private void LoadImage(string path)
        {
            try
            {
                _proc.Load(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Impossible d'ouvrir l'image :\n" + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _origSize = new Size(_proc.OriginalWidth, _proc.OriginalHeight);
            _aspect = _origSize.Height > 0 ? (double)_origSize.Width / _origSize.Height : 1.0;

            _originalBmp?.Dispose();
            var ids = new ProcessSettings
            {
                Width = _origSize.Width,
                Height = _origSize.Height,
                Crop = CropMode.None,
                Format = OutputFormat.Png
            };
            _originalBmp = _proc.RenderPreview(ids);

            _suspend = true;
            numWidth.Value = Clamp(_origSize.Width, (int)numWidth.Minimum, (int)numWidth.Maximum);
            numHeight.Value = Clamp(_origSize.Height, (int)numHeight.Minimum, (int)numHeight.Maximum);
            _suspend = false;

            _manualRectImg = Rectangle.Empty; // no manual selection until the user draws one
            lblOriginal.Text = string.Format("Original : {0} × {1}", _origSize.Width, _origSize.Height);
            btnSave.Enabled = true;

            DoUpdate();
        }

        // ---- Settings ------------------------------------------------------

        private ProcessSettings BuildSettings()
        {
            return new ProcessSettings
            {
                Width = (int)numWidth.Value,
                Height = (int)numHeight.Value,
                KeepRatio = chkKeepRatio.Checked,
                Crop = rbCover.Checked ? CropMode.Cover
                     : rbContain.Checked ? CropMode.Contain
                     : rbManual.Checked ? CropMode.Manual
                     : CropMode.None,
                ManualRect = _manualRectImg,
                Format = (OutputFormat)cmbFormat.SelectedIndex,
                Quality = trkQuality.Value,
                UseTargetSize = chkTargetSize.Checked,
                TargetSizeBytes = (long)numTargetKb.Value * 1024
            };
        }

        private void OnWidthChanged()
        {
            if (_suspend) return;
            if (chkKeepRatio.Checked && _aspect > 0)
            {
                _suspend = true;
                int h = (int)Math.Round((int)numWidth.Value / _aspect);
                numHeight.Value = Clamp(h, (int)numHeight.Minimum, (int)numHeight.Maximum);
                _suspend = false;
            }
            ScheduleUpdate();
        }

        private void OnHeightChanged()
        {
            if (_suspend) return;
            if (chkKeepRatio.Checked && _aspect > 0)
            {
                _suspend = true;
                int w = (int)Math.Round((int)numHeight.Value * _aspect);
                numWidth.Value = Clamp(w, (int)numWidth.Minimum, (int)numWidth.Maximum);
                _suspend = false;
            }
            ScheduleUpdate();
        }

        private void OnCropModeChanged()
        {
            // Keep-ratio only makes sense for "Aucun"; the other modes need an explicit target box.
            bool none = rbNone.Checked;
            chkKeepRatio.Enabled = none;
            if (!none) chkKeepRatio.Checked = false;

            picPreview.Cursor = rbManual.Checked ? Cursors.Cross : Cursors.Default;
            ScheduleUpdate();
        }

        private void UpdateCompressionEnabled()
        {
            var fmt = (OutputFormat)cmbFormat.SelectedIndex;
            bool lossy = fmt == OutputFormat.Jpeg || fmt == OutputFormat.Webp;

            chkTargetSize.Enabled = lossy;
            if (!lossy) chkTargetSize.Checked = false;

            numTargetKb.Enabled = lossy && chkTargetSize.Checked;
            trkQuality.Enabled = lossy && !chkTargetSize.Checked;
            lblQuality.Enabled = lossy && !chkTargetSize.Checked;
        }

        // ---- Preview + estimate -------------------------------------------

        private void ScheduleUpdate()
        {
            if (!_proc.HasImage) return;
            _debounce.Stop();
            _debounce.Start();
        }

        private void DoUpdate()
        {
            if (!_proc.HasImage) return;
            var s = BuildSettings();

            try
            {
                if (s.Crop == CropMode.Manual)
                {
                    picPreview.Image = _originalBmp; // draw selection over the original
                }
                else
                {
                    var bmp = _proc.RenderPreview(s);
                    SwapPreview(bmp);
                }
                picPreview.Invalidate();

                var res = _proc.Render(s);
                UpdateEstimate(res, s);
            }
            catch (Exception ex)
            {
                lblEstimated.Text = "Erreur : " + ex.Message;
            }
        }

        private void SwapPreview(Bitmap bmp)
        {
            picPreview.Image = bmp;
            if (_previewBmp != null && !ReferenceEquals(_previewBmp, bmp))
                _previewBmp.Dispose();
            _previewBmp = bmp;
        }

        private void UpdateEstimate(RenderResult res, ProcessSettings s)
        {
            string text = string.Format("Poids estimé : {0}  ({1} × {2})",
                FormatSize(res.SizeBytes), res.Width, res.Height);

            bool lossy = s.Format == OutputFormat.Jpeg || s.Format == OutputFormat.Webp;
            if (lossy)
                text += "  ·  Q" + res.UsedQuality;
            if (s.UseTargetSize && !res.TargetMet)
                text += "  ⚠ cible non atteinte";

            lblEstimated.Text = text;
        }

        // ---- Saving --------------------------------------------------------

        private void SaveResult()
        {
            if (!_proc.HasImage) return;
            var s = BuildSettings();

            string ext, filter;
            switch (s.Format)
            {
                case OutputFormat.Png: ext = "png"; filter = "PNG|*.png"; break;
                case OutputFormat.Webp: ext = "webp"; filter = "WebP|*.webp"; break;
                case OutputFormat.Gif: ext = "gif"; filter = "GIF|*.gif"; break;
                default: ext = "jpg"; filter = "JPEG|*.jpg"; break;
            }

            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = filter;
                dlg.DefaultExt = ext;
                dlg.FileName = "image." + ext;
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    var res = _proc.Render(s);
                    File.WriteAllBytes(dlg.FileName, res.Bytes);

                    if (s.UseTargetSize && !res.TargetMet)
                        MessageBox.Show(this,
                            "Enregistré, mais le poids cible n'a pas pu être atteint même à qualité minimale (" +
                            FormatSize(res.SizeBytes) + ").\nRéduisez les dimensions ou augmentez la cible.",
                            "Poids cible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Échec de l'enregistrement :\n" + ex.Message,
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // ---- Manual crop (mouse) ------------------------------------------

        private void PicPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (!rbManual.Checked || !_proc.HasImage || e.Button != MouseButtons.Left) return;

            _dragMode = HitTest(e.Location);
            _dragging = true;
            _rectAtDragStart = _manualRectImg;
            _dragStartImg = DisplayToImage(e.Location, false);

            if (_dragMode == DragMode.New)
            {
                Point p = DisplayToImage(e.Location);
                _manualRectImg = new Rectangle(p, Size.Empty);
            }
            picPreview.Invalidate();
        }

        private void PicPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (!rbManual.Checked) return;

            if (!_dragging)
            {
                picPreview.Cursor = CursorFor(HitTest(e.Location)); // hover feedback
                return;
            }

            Point cur = DisplayToImage(e.Location, _dragMode == DragMode.Move);
            _manualRectImg = ApplyDrag(_dragMode, _rectAtDragStart, _dragStartImg, cur);
            picPreview.Invalidate();
        }

        private void PicPreview_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dragging) return;
            _dragging = false;
            _dragMode = DragMode.None;
            if (_manualRectImg.Width < MinCrop || _manualRectImg.Height < MinCrop)
                _manualRectImg = Rectangle.Empty; // discard a too-small selection
            ScheduleUpdate();
        }

        // Computes the new selection for a given drag, clamped to image bounds.
        private Rectangle ApplyDrag(DragMode mode, Rectangle start, Point from, Point to)
        {
            int l = start.Left, t = start.Top, r = start.Right, b = start.Bottom;
            int W = _origSize.Width, H = _origSize.Height;

            switch (mode)
            {
                case DragMode.New:
                    return Rectangle.FromLTRB(
                        Math.Min(from.X, to.X), Math.Min(from.Y, to.Y),
                        Math.Max(from.X, to.X), Math.Max(from.Y, to.Y));

                case DragMode.Move:
                    int dx = to.X - from.X, dy = to.Y - from.Y;
                    l += dx; r += dx; t += dy; b += dy;
                    if (l < 0) { r -= l; l = 0; }
                    if (t < 0) { b -= t; t = 0; }
                    if (r > W) { l -= r - W; r = W; }
                    if (b > H) { t -= b - H; b = H; }
                    return Rectangle.FromLTRB(l, t, r, b);

                case DragMode.W: l = to.X; break;
                case DragMode.E: r = to.X; break;
                case DragMode.N: t = to.Y; break;
                case DragMode.S: b = to.Y; break;
                case DragMode.NW: l = to.X; t = to.Y; break;
                case DragMode.NE: r = to.X; t = to.Y; break;
                case DragMode.SW: l = to.X; b = to.Y; break;
                case DragMode.SE: r = to.X; b = to.Y; break;
            }

            l = Clamp(l, 0, W); r = Clamp(r, 0, W);
            t = Clamp(t, 0, H); b = Clamp(b, 0, H);
            return Rectangle.FromLTRB(Math.Min(l, r), Math.Min(t, b), Math.Max(l, r), Math.Max(t, b));
        }

        // Which part of the selection the point is over (in display coords).
        private DragMode HitTest(Point disp)
        {
            if (_manualRectImg.Width <= 0 || _manualRectImg.Height <= 0) return DragMode.New;
            RectangleF r = ImageRectToDisplay(_manualRectImg);
            if (r.Width <= 0) return DragMode.New;

            bool nearL = Math.Abs(disp.X - r.Left) <= HitPx;
            bool nearR = Math.Abs(disp.X - r.Right) <= HitPx;
            bool nearT = Math.Abs(disp.Y - r.Top) <= HitPx;
            bool nearB = Math.Abs(disp.Y - r.Bottom) <= HitPx;
            bool inX = disp.X >= r.Left - HitPx && disp.X <= r.Right + HitPx;
            bool inY = disp.Y >= r.Top - HitPx && disp.Y <= r.Bottom + HitPx;

            if (nearL && nearT && inX && inY) return DragMode.NW;
            if (nearR && nearT && inX && inY) return DragMode.NE;
            if (nearL && nearB && inX && inY) return DragMode.SW;
            if (nearR && nearB && inX && inY) return DragMode.SE;
            if (nearL && inY) return DragMode.W;
            if (nearR && inY) return DragMode.E;
            if (nearT && inX) return DragMode.N;
            if (nearB && inX) return DragMode.S;
            if (r.Contains(disp)) return DragMode.Move;
            return DragMode.New;
        }

        private static Cursor CursorFor(DragMode m)
        {
            switch (m)
            {
                case DragMode.N:
                case DragMode.S: return Cursors.SizeNS;
                case DragMode.E:
                case DragMode.W: return Cursors.SizeWE;
                case DragMode.NW:
                case DragMode.SE: return Cursors.SizeNWSE;
                case DragMode.NE:
                case DragMode.SW: return Cursors.SizeNESW;
                case DragMode.Move: return Cursors.SizeAll;
                default: return Cursors.Cross;
            }
        }

        private void PicPreview_Paint(object sender, PaintEventArgs e)
        {
            if (!rbManual.Checked || picPreview.Image == null) return;

            RectangleF dispArea = GetImageDisplayRect(_origSize);
            if (dispArea.Width <= 0) return;

            if (_manualRectImg.Width <= 0 || _manualRectImg.Height <= 0)
            {
                // No selection yet: prompt the user to draw one.
                const string hint = "Cliquez-glissez pour dessiner la zone à recadrer";
                using (var font = new Font("Segoe UI", 10f, FontStyle.Bold))
                {
                    SizeF ts = e.Graphics.MeasureString(hint, font);
                    float hx = dispArea.X + (dispArea.Width - ts.Width) / 2f;
                    float hy = dispArea.Y + 10;
                    using (var bg = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
                        e.Graphics.FillRectangle(bg, hx - 6, hy - 3, ts.Width + 12, ts.Height + 6);
                    e.Graphics.DrawString(hint, font, Brushes.White, hx, hy);
                }
                return;
            }

            RectangleF disp = dispArea;

            RectangleF r = ImageRectToDisplay(_manualRectImg);
            var g = e.Graphics;

            using (var pen = new Pen(Color.OrangeRed, 2f))
            using (var dimBrush = new SolidBrush(Color.FromArgb(110, 0, 0, 0)))
            using (var handleBrush = new SolidBrush(Color.White))
            using (var handlePen = new Pen(Color.OrangeRed, 1.5f))
            {
                // Darken everything outside the selection.
                using (Region outside = new Region(disp))
                {
                    outside.Exclude(r);
                    g.FillRegion(dimBrush, outside);
                }
                g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);

                // 8 resize handles.
                foreach (PointF hp in HandlePoints(r))
                {
                    var hr = new RectangleF(hp.X - HandlePx / 2f, hp.Y - HandlePx / 2f, HandlePx, HandlePx);
                    g.FillRectangle(handleBrush, hr);
                    g.DrawRectangle(handlePen, hr.X, hr.Y, hr.Width, hr.Height);
                }

                // Live dimension label (image px).
                string txt = _manualRectImg.Width + " × " + _manualRectImg.Height + " px";
                using (var font = new Font("Segoe UI", 9f, FontStyle.Bold))
                {
                    SizeF ts = g.MeasureString(txt, font);
                    float lx = r.X + 3;
                    float ly = r.Y + 3;
                    if (ly + ts.Height > disp.Bottom) ly = r.Bottom - ts.Height - 3;
                    using (var bg = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                        g.FillRectangle(bg, lx - 2, ly - 1, ts.Width + 4, ts.Height + 2);
                    g.DrawString(txt, font, Brushes.White, lx, ly);
                }
            }
        }

        private static PointF[] HandlePoints(RectangleF r)
        {
            float mx = r.X + r.Width / 2f, my = r.Y + r.Height / 2f;
            return new[]
            {
                new PointF(r.Left, r.Top),  new PointF(mx, r.Top),    new PointF(r.Right, r.Top),
                new PointF(r.Left, my),                                new PointF(r.Right, my),
                new PointF(r.Left, r.Bottom), new PointF(mx, r.Bottom), new PointF(r.Right, r.Bottom)
            };
        }

        private RectangleF ImageRectToDisplay(Rectangle imgRect)
        {
            RectangleF disp = GetImageDisplayRect(_origSize);
            if (disp.Width <= 0 || _origSize.Width <= 0) return RectangleF.Empty;
            float sx = disp.Width / _origSize.Width;
            float sy = disp.Height / _origSize.Height;
            return new RectangleF(
                disp.X + imgRect.X * sx, disp.Y + imgRect.Y * sy,
                imgRect.Width * sx, imgRect.Height * sy);
        }

        private RectangleF GetImageDisplayRect(Size imgSize)
        {
            var cs = picPreview.ClientSize;
            if (imgSize.Width <= 0 || imgSize.Height <= 0 || cs.Width <= 0 || cs.Height <= 0)
                return RectangleF.Empty;

            float scale = Math.Min((float)cs.Width / imgSize.Width, (float)cs.Height / imgSize.Height);
            float w = imgSize.Width * scale;
            float h = imgSize.Height * scale;
            return new RectangleF((cs.Width - w) / 2f, (cs.Height - h) / 2f, w, h);
        }

        private Point DisplayToImage(Point p, bool clamp = true)
        {
            RectangleF r = GetImageDisplayRect(_origSize);
            if (r.Width <= 0 || r.Height <= 0) return Point.Empty;
            int ix = (int)Math.Round((p.X - r.X) / r.Width * _origSize.Width);
            int iy = (int)Math.Round((p.Y - r.Y) / r.Height * _origSize.Height);
            if (clamp)
            {
                ix = Clamp(ix, 0, _origSize.Width);
                iy = Clamp(iy, 0, _origSize.Height);
            }
            return new Point(ix, iy);
        }

        // ---- Drag & drop ---------------------------------------------------

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Length > 0)
                LoadImage(files[0]);
        }

        // ---- Helpers -------------------------------------------------------

        private static int Clamp(int v, int lo, int hi) => v < lo ? lo : (v > hi ? hi : v);

        private static string FormatSize(long bytes)
        {
            if (bytes < 1024) return bytes + " o";
            double kb = bytes / 1024.0;
            if (kb < 1024) return kb.ToString("0.0") + " Ko";
            return (kb / 1024.0).ToString("0.00") + " Mo";
        }

        private void CleanUp()
        {
            _debounce.Stop();
            _tips.Dispose();
            picPreview.Image = null;
            _previewBmp?.Dispose();
            _originalBmp?.Dispose();
            _proc.Dispose();
        }
    }
}
