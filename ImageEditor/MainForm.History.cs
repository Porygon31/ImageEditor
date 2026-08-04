using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ImageEditor
{
    // Cette partie de MainForm contient uniquement la gestion de l'historique.
    // Le mot-clé "partial" permet de répartir une même classe dans plusieurs fichiers.
    // Cela évite de rendre MainForm.cs encore plus long et plus difficile à lire.
    public partial class MainForm
    {
        // On conserve au maximum 100 étapes.
        // Une étape contient seulement quelques nombres et options, jamais une copie de l'image.
        private const int MaxHistoryStates = 100;

        // Lorsqu'un curseur ou une valeur change rapidement, on attend 350 ms avant
        // d'enregistrer l'étape. Ainsi, déplacer la qualité de 85 à 70 produit une seule
        // action dans l'historique au lieu de quinze petites actions.
        private readonly Timer _historyDebounce = new Timer { Interval = 350 };

        // _undoHistory contient les états vers lesquels on peut revenir avec Annuler.
        // _redoHistory contient les états que l'on peut remettre après une annulation.
        private readonly List<EditorState> _undoHistory = new List<EditorState>();
        private readonly List<EditorState> _redoHistory = new List<EditorState>();

        // Dernier état déjà enregistré dans l'historique.
        private EditorState _recordedState;

        // Référence de l'image pour laquelle l'historique a été créé.
        // Quand une nouvelle image est ouverte, _originalBmp change de référence :
        // cela nous permet de vider automatiquement l'ancien historique.
        private Bitmap _historyImageReference;

        // Pendant un Undo/Redo, plusieurs contrôles changent automatiquement.
        // Ce booléen empêche ces changements automatiques d'être ajoutés comme de nouvelles actions.
        private bool _applyingHistory;

        // Les deux boutons sont créés par code pour ne pas alourdir davantage le Designer WinForms.
        private ModernButton _btnUndo;
        private ModernButton _btnRedo;

        /// <summary>
        /// Représente une "photo" très légère de tous les réglages modifiables.
        /// Aucun pixel n'est stocké ici : seulement les valeurs de l'interface.
        /// </summary>
        private sealed class EditorState
        {
            public int Width;
            public int Height;
            public bool KeepRatio;
            public CropMode Crop;
            public Rectangle ManualRect;
            public int FormatIndex;
            public int Quality;
            public bool UseTargetSize;
            public decimal TargetSizeKb;

            // Compare deux états pour savoir si l'utilisateur a réellement changé quelque chose.
            public bool HasSameValues(EditorState other)
            {
                if (other == null) return false;

                return Width == other.Width
                    && Height == other.Height
                    && KeepRatio == other.KeepRatio
                    && Crop == other.Crop
                    && ManualRect == other.ManualRect
                    && FormatIndex == other.FormatIndex
                    && Quality == other.Quality
                    && UseTargetSize == other.UseTargetSize
                    && TargetSizeKb == other.TargetSizeKb;
            }
        }

        /// <summary>
        /// OnLoad est appelé une fois lorsque la fenêtre est prête à être affichée.
        /// C'est le bon moment pour créer les boutons et écouter les changements des contrôles.
        /// </summary>
        protected override void OnLoad(EventArgs eventArgs)
        {
            base.OnLoad(eventArgs);

            CreateHistoryButtons();
            SubscribeToHistoryEvents();

            _historyDebounce.Tick += HistoryDebounce_Tick;
            FormClosing += (sender, args) => _historyDebounce.Stop();
            FormClosed += (sender, args) => _historyDebounce.Dispose();

            UpdateHistoryButtons();
        }

        /// <summary>
        /// Intercepte les raccourcis avant qu'un NumericUpDown ou un autre contrôle ne les utilise.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message message, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Z))
            {
                UndoEditorChange();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Y)
                || keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                RedoEditorChange();
                return true;
            }

            return base.ProcessCmdKey(ref message, keyData);
        }

        private void CreateHistoryButtons()
        {
            _btnUndo = CreateHistoryButton("↶  Annuler");
            _btnRedo = CreateHistoryButton("↷  Rétablir");

            _btnUndo.Click += (sender, eventArgs) => UndoEditorChange();
            _btnRedo.Click += (sender, eventArgs) => RedoEditorChange();

            pnlPreviewHeader.Controls.Add(_btnUndo);
            pnlPreviewHeader.Controls.Add(_btnRedo);

            // Les boutons restent alignés à droite lorsque la fenêtre change de taille.
            pnlPreviewHeader.Resize += (sender, eventArgs) => PositionHistoryButtons();
            PositionHistoryButtons();

            _tips.SetToolTip(_btnUndo, "Annuler le dernier changement (Ctrl+Z)");
            _tips.SetToolTip(_btnRedo, "Rétablir le changement annulé (Ctrl+Y ou Ctrl+Maj+Z)");
        }

        private static ModernButton CreateHistoryButton(string text)
        {
            return new ModernButton
            {
                Text = text,
                Size = new Size(106, 34),
                CornerRadius = 10,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(51, 65, 85),
                HoverBackColor = Color.FromArgb(71, 85, 105),
                PressedBackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.FromArgb(248, 250, 252),
                DisabledBackColor = Color.FromArgb(30, 41, 59),
                DisabledTextColor = Color.FromArgb(100, 116, 139),
                TabStop = false
            };
        }

        private void PositionHistoryButtons()
        {
            if (_btnUndo == null || _btnRedo == null) return;

            const int rightMargin = 28;
            const int gap = 8;

            _btnRedo.Location = new Point(
                pnlPreviewHeader.ClientSize.Width - rightMargin - _btnRedo.Width,
                18);

            _btnUndo.Location = new Point(
                _btnRedo.Left - gap - _btnUndo.Width,
                18);
        }

        private void SubscribeToHistoryEvents()
        {
            // Tous ces événements sont déclenchés après la modification d'un contrôle.
            numWidth.ValueChanged += HistoryControlChanged;
            numHeight.ValueChanged += HistoryControlChanged;
            chkKeepRatio.CheckedChanged += HistoryControlChanged;

            rbNone.CheckedChanged += HistoryControlChanged;
            rbCover.CheckedChanged += HistoryControlChanged;
            rbContain.CheckedChanged += HistoryControlChanged;
            rbManual.CheckedChanged += HistoryControlChanged;

            cmbFormat.SelectedIndexChanged += HistoryControlChanged;
            trkQuality.ValueChanged += HistoryControlChanged;
            chkTargetSize.CheckedChanged += HistoryControlChanged;
            numTargetKb.ValueChanged += HistoryControlChanged;

            // Le rectangle du recadrage manuel n'est pas un contrôle classique.
            // On l'enregistre donc lorsque l'utilisateur relâche la souris.
            picPreview.MouseUp += HistoryControlChanged;

            // Après une ouverture ou un glisser-déposer, on vérifie immédiatement
            // si l'image a changé afin de créer un nouveau point de départ.
            btnOpen.Click += HistoryImageMayHaveChanged;
            DragDrop += HistoryImageMayHaveChanged;
        }

        private void HistoryControlChanged(object sender, EventArgs eventArgs)
        {
            if (_applyingHistory || _suspend || _closing || !_proc.HasImage)
                return;

            // Chaque nouveau changement repousse l'enregistrement de 350 ms.
            // Les modifications rapides sont donc regroupées en une seule étape logique.
            _historyDebounce.Stop();
            _historyDebounce.Start();
        }

        private void HistoryDebounce_Tick(object sender, EventArgs eventArgs)
        {
            _historyDebounce.Stop();
            RecordCurrentStateIfChanged();
        }

        private void HistoryImageMayHaveChanged(object sender, EventArgs eventArgs)
        {
            QueueImageHistoryReset();
        }

        /// <summary>
        /// Attend la fin de l'événement d'ouverture avant de comparer l'image.
        /// Le gestionnaire de MainForm.cs aura ainsi déjà terminé LoadImage().
        /// </summary>
        private void QueueImageHistoryReset()
        {
            if (_closing || IsDisposed || Disposing || !IsHandleCreated)
                return;

            try
            {
                // EnsureHistoryMatchesCurrentImage retourne un booléen.
                // Une expression lambda permet de l'appeler depuis un Action (qui retourne void)
                // tout en ignorant volontairement cette valeur de retour ici.
                BeginInvoke(new Action(() => EnsureHistoryMatchesCurrentImage()));
            }
            catch (InvalidOperationException)
            {
                // La fenêtre est probablement en train de se fermer.
                // Dans ce cas, il n'y a simplement plus rien à enregistrer.
            }
        }

        private void RecordCurrentStateIfChanged()
        {
            if (_applyingHistory || _suspend || _closing || !_proc.HasImage)
                return;

            // Retourne false lorsqu'une nouvelle image vient d'être détectée.
            // Son état actuel devient alors le nouveau point de départ.
            if (!EnsureHistoryMatchesCurrentImage())
                return;

            EditorState currentState = CaptureEditorState();

            if (_recordedState == null)
            {
                _recordedState = currentState;
                UpdateHistoryButtons();
                return;
            }

            if (_recordedState.HasSameValues(currentState))
                return;

            AddHistoryState(_undoHistory, _recordedState);
            _recordedState = currentState;

            // Dès qu'une nouvelle modification est faite après un Undo,
            // l'ancienne branche de Redo n'est plus valable.
            _redoHistory.Clear();
            UpdateHistoryButtons();
        }

        private bool EnsureHistoryMatchesCurrentImage()
        {
            if (!_proc.HasImage || _originalBmp == null)
            {
                ClearHistory();
                return false;
            }

            if (ReferenceEquals(_historyImageReference, _originalBmp))
                return true;

            // Une autre image a été chargée : l'historique précédent ne s'applique plus.
            _historyDebounce.Stop();
            _historyImageReference = _originalBmp;
            _undoHistory.Clear();
            _redoHistory.Clear();
            _recordedState = CaptureEditorState();
            UpdateHistoryButtons();
            return false;
        }

        private void ClearHistory()
        {
            _historyDebounce.Stop();
            _historyImageReference = null;
            _recordedState = null;
            _undoHistory.Clear();
            _redoHistory.Clear();
            UpdateHistoryButtons();
        }

        private EditorState CaptureEditorState()
        {
            return new EditorState
            {
                Width = (int)numWidth.Value,
                Height = (int)numHeight.Value,
                KeepRatio = chkKeepRatio.Checked,
                Crop = rbCover.Checked ? CropMode.Cover
                     : rbContain.Checked ? CropMode.Contain
                     : rbManual.Checked ? CropMode.Manual
                     : CropMode.None,
                ManualRect = _manualRectImg,
                FormatIndex = cmbFormat.SelectedIndex,
                Quality = trkQuality.Value,
                UseTargetSize = chkTargetSize.Checked,
                TargetSizeKb = numTargetKb.Value
            };
        }

        private void UndoEditorChange()
        {
            if (!_proc.HasImage || _closing)
                return;

            // Enregistre d'abord une éventuelle modification dont le délai de 350 ms
            // n'est pas encore terminé. L'utilisateur peut donc faire Ctrl+Z immédiatement.
            _historyDebounce.Stop();
            RecordCurrentStateIfChanged();

            if (_undoHistory.Count == 0)
                return;

            EditorState currentState = CaptureEditorState();
            EditorState previousState = PopLastHistoryState(_undoHistory);

            AddHistoryState(_redoHistory, currentState);
            ApplyEditorState(previousState);
        }

        private void RedoEditorChange()
        {
            if (!_proc.HasImage || _closing)
                return;

            _historyDebounce.Stop();
            RecordCurrentStateIfChanged();

            if (_redoHistory.Count == 0)
                return;

            EditorState currentState = CaptureEditorState();
            EditorState nextState = PopLastHistoryState(_redoHistory);

            AddHistoryState(_undoHistory, currentState);
            ApplyEditorState(nextState);
        }

        private void ApplyEditorState(EditorState state)
        {
            if (state == null) return;

            _applyingHistory = true;

            try
            {
                // On arrête les calculs de l'ancien état avant de modifier les contrôles.
                _historyDebounce.Stop();
                _debounce.Stop();
                CancelCurrentUpdate();

                // _suspend empêche les gestionnaires Width/Height de recalculer le ratio
                // pendant que toutes les valeurs sont restaurées.
                _suspend = true;

                _manualRectImg = state.ManualRect;

                // Le mode de recadrage est restauré avant "Garder le ratio",
                // car OnCropModeChanged peut désactiver cette case.
                rbNone.Checked = state.Crop == CropMode.None;
                rbCover.Checked = state.Crop == CropMode.Cover;
                rbContain.Checked = state.Crop == CropMode.Contain;
                rbManual.Checked = state.Crop == CropMode.Manual;

                numWidth.Value = ClampNumericValue(state.Width, numWidth);
                numHeight.Value = ClampNumericValue(state.Height, numHeight);

                int formatIndex = Clamp(state.FormatIndex, 0, cmbFormat.Items.Count - 1);
                cmbFormat.SelectedIndex = formatIndex;

                trkQuality.Value = Clamp(state.Quality, trkQuality.Minimum, trkQuality.Maximum);
                numTargetKb.Value = ClampNumericValue(state.TargetSizeKb, numTargetKb);

                chkKeepRatio.Checked = state.KeepRatio && state.Crop == CropMode.None;
                chkTargetSize.Checked = state.UseTargetSize && IsLossyFormat(formatIndex);
            }
            finally
            {
                _suspend = false;
                _applyingHistory = false;
            }

            // Remet à jour les contrôles dont l'état Enabled dépend du format ou du recadrage.
            chkKeepRatio.Enabled = rbNone.Checked;
            UpdateCompressionEnabled();
            lblQuality.Text = "Qualité : " + trkQuality.Value;
            picPreview.Cursor = rbManual.Checked ? Cursors.Cross : Cursors.Default;
            picPreview.Invalidate();

            _recordedState = state;
            UpdateHistoryButtons();

            // Les événements déclenchés pendant la restauration ont pu démarrer le timer.
            // On l'arrête et on lance une seule actualisation pour l'état final.
            _historyDebounce.Stop();
            _debounce.Stop();
            CancelCurrentUpdate();
            _ = DoUpdateAsync();
        }

        private static bool IsLossyFormat(int formatIndex)
        {
            return formatIndex == (int)OutputFormat.Jpeg
                || formatIndex == (int)OutputFormat.Webp;
        }

        private static decimal ClampNumericValue(decimal value, NumericUpDown control)
        {
            if (value < control.Minimum) return control.Minimum;
            if (value > control.Maximum) return control.Maximum;
            return value;
        }

        private static void AddHistoryState(List<EditorState> history, EditorState state)
        {
            if (state == null) return;

            history.Add(state);

            // Supprime l'étape la plus ancienne lorsque la limite est dépassée.
            if (history.Count > MaxHistoryStates)
                history.RemoveAt(0);
        }

        private static EditorState PopLastHistoryState(List<EditorState> history)
        {
            int lastIndex = history.Count - 1;
            EditorState state = history[lastIndex];
            history.RemoveAt(lastIndex);
            return state;
        }

        private void UpdateHistoryButtons()
        {
            if (_btnUndo == null || _btnRedo == null)
                return;

            bool hasImage = _proc.HasImage && !_closing;
            _btnUndo.Enabled = hasImage && _undoHistory.Count > 0;
            _btnRedo.Enabled = hasImage && _redoHistory.Count > 0;

            _tips.SetToolTip(_btnUndo,
                _undoHistory.Count > 0
                    ? "Annuler le dernier changement — " + _undoHistory.Count + " étape(s) disponible(s) (Ctrl+Z)"
                    : "Aucun changement à annuler (Ctrl+Z)");

            _tips.SetToolTip(_btnRedo,
                _redoHistory.Count > 0
                    ? "Rétablir le changement — " + _redoHistory.Count + " étape(s) disponible(s) (Ctrl+Y)"
                    : "Aucun changement à rétablir (Ctrl+Y)");
        }
    }
}
