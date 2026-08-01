using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor
{
    public partial class MainForm : Form
    {
        private readonly ImageProcessor _proc = new ImageProcessor();
        private readonly Timer _debounce = new Timer { Interval = 200 };
        private readonly ToolTip _tips = new ToolTip { AutoPopDelay = 15000, InitialDelay = 400, ReshowDelay = 100 };

        // Cette source permet d'annuler logiquement l'ancien calcul lorsqu'un nouveau commence.
        // ImageSharp ne peut pas toujours stopper un encodage déjà commencé, mais le résultat
        // devenu inutile sera ignoré et ne remplacera pas le dernier aperçu demandé.
        private CancellationTokenSource _updateCancellation;

        // ImageProcessor contient une seule image originale partagée.
        // Ce verrou garantit qu'un seul traitement l'utilise à la fois.
        private readonly SemaphoreSlim _processorLock = new SemaphoreSlim(1, 1);

        private Bitmap _originalBmp;   // original, for manual-crop display
        private Bitmap _previewBmp;    // generated result preview (owned)
        private Size _origSize;
        private double _aspect = 1.0;
        private bool _suspend;          // guards programmatic control updates
        private bool _closing;

        // Manual crop selection, in ORIGINAL image pixel coordinates.
        private Rectangle _manualRectImg;
        private bool _dragging;
        private Point _dragStartImg;
        private Rectangle _rectAtDragStart;
        private DragMode _dragMode = DragMode.None;

        private const int HandlePx = 8;
        private const int HitPx = 7;
        private const int MinCrop = 4;

        private enum DragMode { None, New, Move, N, S, E, W, NE, NW, SE, SW }

        // Petit objet utilisé pour renvoyer les deux résultats produits en arrière-plan.
        private sealed class AsyncRenderResult
        {
            public Bitmap Preview;
            public RenderResult Estimate;
        }

        public MainForm()
        {
            InitializeComponent();

            cmbFormat.SelectedIndex = 1; // JPEG

            btnOpen.Click += (s, e) => OpenViaDialog();
            btnSave.Click += async (s, e) => await SaveResultAsync();

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

            DragEnter += MainForm_DragEnter;
            DragDrop += MainForm_DragDrop;
            FormClosed += (s, e) => CleanUp();

            // Le Timer attend 200 ms après la dernière modification.
            // Cela évite de relancer un gros calcul à chaque petit mouvement du curseur.
            _debounce.Tick += async (s, e) =>
            {
                _debounce.Stop();
                await DoUpdateAsync();
            };

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
            // Un ancien aperçu ne doit pas arriver après le chargement de la nouvelle image.
            CancelCurrentUpdate();

            try
            {
                // LoadImage reste synchrone car il n'est exécuté qu'une fois à l'ouverture.
                // Le verrou empêche un ancien rendu de lire l'image pendant son remplacement.
                _processorLock.Wait();
                try
                {
                    _proc.Load(path);
                }
                finally
                {
                    _processorLock.Release();
                }
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

            _processorLock.Wait();
            try
            {
                _originalBmp = _proc.RenderPreview(ids);
            }
            finally
            {
                _processorLock.Release();
            }

            _suspend = true;
            numWidth.Value = Clamp(_origSize.Width, (int)numWidth.Minimum, (int)numWidth.Maximum);
            numHeight.Value = Clamp(_origSize.Height, (int)numHeight.Minimum, (int)numHeight.Maximum);
            _suspend = false;

            _manualRectImg = Rectangle.Empty;
            lblOriginal.Text = string.Format("Original : {0} × {1}", _origSize.Width, _origSize.Height);
            btnSave.Enabled = true;

            // On lance l'actualisation sans bloquer la fenêtre.
            _ = DoUpdateAsync();
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
            if (!_proc.HasImage || _closing) return;

            // Dès qu'un réglage change, l'ancien résultat n'est plus utile.
            CancelCurrentUpdate();

            _debounce.Stop();
            _debounce.Start();
        }

        private void CancelCurrentUpdate()
        {
            if (_updateCancellation == null) return;

            _updateCancellation.Cancel();
            _updateCancellation.Dispose();
            _updateCancellation = null;
        }

        private async Task DoUpdateAsync()
        {
            if (!_proc.HasImage || _closing) return;

            CancelCurrentUpdate();
            _updateCancellation = new CancellationTokenSource();
            CancellationToken token = _updateCancellation.Token;

            // BuildSettings lit les contrôles WinForms. Il faut donc le faire sur le thread de l'interface.
            ProcessSettings settings = BuildSettings();
            bool manualPreview = settings.Crop == CropMode.Manual;

            lblEstimated.Text = "Calcul en cours…";

            try
            {
                // Task.Run envoie le travail lourd sur un thread d'arrière-plan.
                // Pendant ce temps, la fenêtre reste réactive.
                AsyncRenderResult result = await Task.Run(async () =>
                {
                    token.ThrowIfCancellationRequested();

                    await _processorLock.WaitAsync(token);
                    try
                    {
                        token.ThrowIfCancellationRequested();

                        var rendered = new AsyncRenderResult();

                        // En mode manuel, l'interface affiche l'original avec le rectangle par-dessus.
                        // Il est donc inutile de générer un second Bitmap d'aperçu.
                        if (!manualPreview)
                            rendered.Preview = _proc.RenderPreview(settings);

                        token.ThrowIfCancellationRequested();
                        rendered.Estimate = _proc.Render(settings);
                        token.ThrowIfCancellationRequested();

                        return rendered;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        _processorLock.Release();
                    }
                }, token);

                // Après await, on revient automatiquement sur le thread WinForms.
                // On peut donc modifier les contrôles sans Invoke().
                token.ThrowIfCancellationRequested();
                if (_closing)
                {
                    result.Preview?.Dispose();
                    return;
                }

                if (manualPreview)
                {
                    picPreview.Image = _originalBmp;
                }
                else
                {
                    SwapPreview(result.Preview);
                    result.Preview = null;
                }

                picPreview.Invalidate();
                UpdateEstimate(result.Estimate, settings);
            }
            catch (OperationCanceledException)
            {
                // Comportement normal : l'utilisateur a changé un réglage avant la fin.
                // On ne montre donc aucune erreur.
            }
            catch (Exception ex)
            {
                if (!_closing)
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

        private async Task SaveResultAsync()
        {
            if (!_proc.HasImage) return;
            var settings = BuildSettings();

            string ext, filter;
            switch (settings.Format)
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

                btnSave.Enabled = false;
                string previousText = btnSave.Text;
                btnSave.Text = "Enregistrement…";

                try
                {
                    // Le rendu et l'écriture du fichier peuvent être longs.
                    // Ils sont donc eux aussi réalisés en arrière-plan.
                    RenderResult result = await Task.Run(async () =>
                    {
                        await _processorLock.WaitAsync();
                        try
                        {
                            RenderResult rendered = _proc.Render(settings);
                            File.WriteAllBytes(dlg.FileName, rendered.Bytes);
                            return rendered;
                        }
                        finally
                        {
                            _processorLock.Release();
                        }
                    });

                    if (settings.UseTargetSize && !result.TargetMet)
                    {
                        MessageBox.Show(this,
                            "Enregistré, mais le poids cible n'a pas pu être atteint même à qualité minimale (" +
                            FormatSize(result.SizeBytes) + ").\nRéduisez les dimensions ou augmentez la cible.",
                            "Poids cible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Échec de l'enregistrement :\n" + ex.Message,
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    if (!_closing)
                    {
                        btnSave.Text = previousText;
                        btnSave.Enabled = true;
                    }
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
                picPreview.Cursor = CursorFor(HitTest(e.Location));
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
                _manualRectImg = Rectangle.Empty;
            ScheduleUpdate();
        }

        private Rectangle ApplyDrag(DragMode mode, Rectangle start, Point from, Point to)
        {
            int l = start.Left, t = start.Top, r = start.Right, b = start.Bottom;
            int width = _origSize.Width, height = _origSize.Height;

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
                    if (r > width) { l -= r - width; r = width; }
                    if (b > height) { t -= b - height; b = height; }
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

            l = Clamp(l, 0, width); r = Clamp(r, 0, width);
            t = Clamp(t, 0, height); b = Clamp(b, 0, height);
            return Rectangle.FromLTRB(Math.Min(l, r), Math.Min(t, b), Math.Max(l, r), Math.Max(t, b));
        }

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

        private static Cursor CursorFor(DragMode mode)
        {
            switch (mode)
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
                const string hint = "Cliquez-glissez pour dessiner la zone à recadrer";
                using (var font = new Font("Segoe UI", 10f, FontStyle.Bold))
                {
                    SizeF textSize = e.Graphics.MeasureString(hint, font);
                    float x = dispArea.X + (dispArea.Width - textSize.Width) / 2f;
                    float y = dispArea.Y + 10;
                    using (var background = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
                        e.Graphics.FillRectangle(background, x - 6, y - 3, textSize.Width + 12, textSize.Height + 6);
                    e.Graphics.DrawString(hint, font, Brushes.White, x, y);
                }
                return;
            }

            RectangleF selection = ImageRectToDisplay(_manualRectImg);
            var graphics = e.Graphics;

            using (var pen = new Pen(Color.OrangeRed, 2f))
            using (var dimBrush = new SolidBrush(Color.FromArgb(110, 0, 0, 0)))
            using (var handleBrush = new SolidBrush(Color.White))
            using (var handlePen = new Pen(Color.OrangeRed, 1.5f))
            {
                using (Region outside = new Region(dispArea))
                {
                    outside.Exclude(selection);
                    graphics.FillRegion(dimBrush, outside);
                }

                graphics.DrawRectangle(pen, selection.X, selection.Y, selection.Width, selection.Height);

                foreach (PointF point in HandlePoints(selection))
                {
                    var handle = new RectangleF(point.X - HandlePx / 2f, point.Y - HandlePx / 2f, HandlePx, HandlePx);
                    graphics.FillRectangle(handleBrush, handle);
                    graphics.DrawRectangle(handlePen, handle.X, handle.Y, handle.Width, handle.Height);
                }

                string text = _manualRectImg.Width + " × " + _manualRectImg.Height + " px";
                using (var font = new Font("Segoe UI", 9f, FontStyle.Bold))
                {
                    SizeF textSize = graphics.MeasureString(text, font);
                    float x = selection.X + 3;
                    float y = selection.Y + 3;
                    if (y + textSize.Height > dispArea.Bottom)
                        y = selection.Bottom - textSize.Height - 3;

                    using (var background = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                        graphics.FillRectangle(background, x - 2, y - 1, textSize.Width + 4, textSize.Height + 2);
                    graphics.DrawString(text, font, Brushes.White, x, y);
                }
            }
        }

        private static PointF[] HandlePoints(RectangleF r)
        {
            float middleX = r.X + r.Width / 2f;
            float middleY = r.Y + r.Height / 2f;
            return new[]
            {
                new PointF(r.Left, r.Top), new PointF(middleX, r.Top), new PointF(r.Right, r.Top),
                new PointF(r.Left, middleY), new PointF(r.Right, middleY),
                new PointF(r.Left, r.Bottom), new PointF(middleX, r.Bottom), new PointF(r.Right, r.Bottom)
            };
        }

        private RectangleF ImageRectToDisplay(Rectangle imgRect)
        {
            RectangleF disp = GetImageDisplayRect(_origSize);
            if (disp.Width <= 0 || _origSize.Width <= 0) return RectangleF.Empty;
            float scaleX = disp.Width / _origSize.Width;
            float scaleY = disp.Height / _origSize.Height;
            return new RectangleF(
                disp.X + imgRect.X * scaleX,
                disp.Y + imgRect.Y * scaleY,
                imgRect.Width * scaleX,
                imgRect.Height * scaleY);
        }

        private RectangleF GetImageDisplayRect(Size imgSize)
        {
            var clientSize = picPreview.ClientSize;
            if (imgSize.Width <= 0 || imgSize.Height <= 0 || clientSize.Width <= 0 || clientSize.Height <= 0)
                return RectangleF.Empty;

            float scale = Math.Min((float)clientSize.Width / imgSize.Width, (float)clientSize.Height / imgSize.Height);
            float width = imgSize.Width * scale;
            float height = imgSize.Height * scale;
            return new RectangleF((clientSize.Width - width) / 2f, (clientSize.Height - height) / 2f, width, height);
        }

        private Point DisplayToImage(Point point, bool clamp = true)
        {
            RectangleF display = GetImageDisplayRect(_origSize);
            if (display.Width <= 0 || display.Height <= 0) return Point.Empty;

            int x = (int)Math.Round((point.X - display.X) / display.Width * _origSize.Width);
            int y = (int)Math.Round((point.Y - display.Y) / display.Height * _origSize.Height);

            if (clamp)
            {
                x = Clamp(x, 0, _origSize.Width);
                y = Clamp(y, 0, _origSize.Height);
            }

            return new Point(x, y);
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

        private static int Clamp(int value, int minimum, int maximum)
        {
            return value < minimum ? minimum : (value > maximum ? maximum : value);
        }

        private static string FormatSize(long bytes)
        {
            if (bytes < 1024) return bytes + " o";
            double kb = bytes / 1024.0;
            if (kb < 1024) return kb.ToString("0.0") + " Ko";
            return (kb / 1024.0).ToString("0.00") + " Mo";
        }

        private void CleanUp()
        {
            _closing = true;
            _debounce.Stop();
            CancelCurrentUpdate();
            _tips.Dispose();
            picPreview.Image = null;
            _previewBmp?.Dispose();
            _originalBmp?.Dispose();

            // On évite de supprimer ImageProcessor pendant qu'un thread l'utilise encore.
            _processorLock.Wait();
            try
            {
                _proc.Dispose();
            }
            finally
            {
                _processorLock.Release();
                _processorLock.Dispose();
            }
        }
    }
}
