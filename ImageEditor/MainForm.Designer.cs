namespace ImageEditor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlPreviewArea = new System.Windows.Forms.Panel();
            this.pnlPreviewPadding = new System.Windows.Forms.Panel();
            this.pnlPreviewCard = new ImageEditor.RoundedPanel();
            this.picPreview = new ImageEditor.PreviewPictureBox();
            this.pnlPreviewFooter = new System.Windows.Forms.Panel();
            this.lblDropHint = new System.Windows.Forms.Label();
            this.pnlPreviewHeader = new System.Windows.Forms.Panel();
            this.lblPreviewSubtitle = new System.Windows.Forms.Label();
            this.lblPreviewTitle = new System.Windows.Forms.Label();
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.pnlSettingsScroll = new System.Windows.Forms.Panel();
            this.grpComp = new ImageEditor.RoundedPanel();
            this.lblCompHelp = new System.Windows.Forms.Label();
            this.lblCompTitle = new System.Windows.Forms.Label();
            this.lblQuality = new System.Windows.Forms.Label();
            this.trkQuality = new System.Windows.Forms.TrackBar();
            this.chkTargetSize = new System.Windows.Forms.CheckBox();
            this.numTargetKb = new System.Windows.Forms.NumericUpDown();
            this.lblKb = new System.Windows.Forms.Label();
            this.grpFormat = new ImageEditor.RoundedPanel();
            this.lblFormatHelp = new System.Windows.Forms.Label();
            this.lblFormatTitle = new System.Windows.Forms.Label();
            this.cmbFormat = new System.Windows.Forms.ComboBox();
            this.grpCrop = new ImageEditor.RoundedPanel();
            this.lblCropHelp = new System.Windows.Forms.Label();
            this.lblCropTitle = new System.Windows.Forms.Label();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbCover = new System.Windows.Forms.RadioButton();
            this.rbContain = new System.Windows.Forms.RadioButton();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.grpDim = new ImageEditor.RoundedPanel();
            this.lblDimHelp = new System.Windows.Forms.Label();
            this.lblDimTitle = new System.Windows.Forms.Label();
            this.lblW = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.lblH = new System.Windows.Forms.Label();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.chkKeepRatio = new System.Windows.Forms.CheckBox();
            this.pnlFileInfo = new ImageEditor.RoundedPanel();
            this.pnlFileAccent = new System.Windows.Forms.Panel();
            this.lblFileCaption = new System.Windows.Forms.Label();
            this.lblOriginal = new System.Windows.Forms.Label();
            this.btnOpen = new ImageEditor.ModernButton();
            this.pnlActionBar = new System.Windows.Forms.Panel();
            this.btnSave = new ImageEditor.ModernButton();
            this.lblEstimated = new System.Windows.Forms.Label();
            this.pnlSidebarHeader = new System.Windows.Forms.Panel();
            this.pnlLogo = new ImageEditor.RoundedPanel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblAppSubtitle = new System.Windows.Forms.Label();
            this.lblAppName = new System.Windows.Forms.Label();
            this.pnlPreviewArea.SuspendLayout();
            this.pnlPreviewPadding.SuspendLayout();
            this.pnlPreviewCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.pnlPreviewFooter.SuspendLayout();
            this.pnlPreviewHeader.SuspendLayout();
            this.pnlSettings.SuspendLayout();
            this.pnlSettingsScroll.SuspendLayout();
            this.grpComp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkQuality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetKb)).BeginInit();
            this.grpFormat.SuspendLayout();
            this.grpCrop.SuspendLayout();
            this.grpDim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.pnlFileInfo.SuspendLayout();
            this.pnlActionBar.SuspendLayout();
            this.pnlSidebarHeader.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPreviewArea
            // 
            this.pnlPreviewArea.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlPreviewArea.Controls.Add(this.pnlPreviewPadding);
            this.pnlPreviewArea.Controls.Add(this.pnlPreviewFooter);
            this.pnlPreviewArea.Controls.Add(this.pnlPreviewHeader);
            this.pnlPreviewArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPreviewArea.Location = new System.Drawing.Point(0, 0);
            this.pnlPreviewArea.Name = "pnlPreviewArea";
            this.pnlPreviewArea.Size = new System.Drawing.Size(790, 760);
            this.pnlPreviewArea.TabIndex = 0;
            // 
            // pnlPreviewPadding
            // 
            this.pnlPreviewPadding.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlPreviewPadding.Controls.Add(this.pnlPreviewCard);
            this.pnlPreviewPadding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPreviewPadding.Location = new System.Drawing.Point(0, 88);
            this.pnlPreviewPadding.Name = "pnlPreviewPadding";
            this.pnlPreviewPadding.Padding = new System.Windows.Forms.Padding(28, 12, 28, 16);
            this.pnlPreviewPadding.Size = new System.Drawing.Size(790, 620);
            this.pnlPreviewPadding.TabIndex = 1;
            // 
            // pnlPreviewCard
            // 
            this.pnlPreviewCard.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.pnlPreviewCard.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.pnlPreviewCard.BorderSize = 1;
            this.pnlPreviewCard.Controls.Add(this.picPreview);
            this.pnlPreviewCard.CornerRadius = 20;
            this.pnlPreviewCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPreviewCard.Location = new System.Drawing.Point(28, 12);
            this.pnlPreviewCard.Name = "pnlPreviewCard";
            this.pnlPreviewCard.Padding = new System.Windows.Forms.Padding(2);
            this.pnlPreviewCard.Size = new System.Drawing.Size(734, 592);
            this.pnlPreviewCard.TabIndex = 0;
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPreview.Location = new System.Drawing.Point(2, 2);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(730, 588);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;
            // 
            // pnlPreviewFooter
            // 
            this.pnlPreviewFooter.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlPreviewFooter.Controls.Add(this.lblDropHint);
            this.pnlPreviewFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPreviewFooter.Location = new System.Drawing.Point(0, 708);
            this.pnlPreviewFooter.Name = "pnlPreviewFooter";
            this.pnlPreviewFooter.Size = new System.Drawing.Size(790, 52);
            this.pnlPreviewFooter.TabIndex = 2;
            // 
            // lblDropHint
            // 
            this.lblDropHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDropHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDropHint.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblDropHint.Location = new System.Drawing.Point(0, 0);
            this.lblDropHint.Name = "lblDropHint";
            this.lblDropHint.Size = new System.Drawing.Size(790, 52);
            this.lblDropHint.TabIndex = 0;
            this.lblDropHint.Text = "Glisse-dépose une image n’importe où dans la fenêtre";
            this.lblDropHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlPreviewHeader
            // 
            this.pnlPreviewHeader.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlPreviewHeader.Controls.Add(this.lblPreviewSubtitle);
            this.pnlPreviewHeader.Controls.Add(this.lblPreviewTitle);
            this.pnlPreviewHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreviewHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlPreviewHeader.Name = "pnlPreviewHeader";
            this.pnlPreviewHeader.Size = new System.Drawing.Size(790, 88);
            this.pnlPreviewHeader.TabIndex = 0;
            // 
            // lblPreviewSubtitle
            // 
            this.lblPreviewSubtitle.AutoSize = true;
            this.lblPreviewSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPreviewSubtitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblPreviewSubtitle.Location = new System.Drawing.Point(28, 53);
            this.lblPreviewSubtitle.Name = "lblPreviewSubtitle";
            this.lblPreviewSubtitle.Size = new System.Drawing.Size(322, 17);
            this.lblPreviewSubtitle.TabIndex = 1;
            this.lblPreviewSubtitle.Text = "Aperçu en temps réel du recadrage et de la compression";
            // 
            // lblPreviewTitle
            // 
            this.lblPreviewTitle.AutoSize = true;
            this.lblPreviewTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblPreviewTitle.ForeColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.lblPreviewTitle.Location = new System.Drawing.Point(25, 18);
            this.lblPreviewTitle.Name = "lblPreviewTitle";
            this.lblPreviewTitle.Size = new System.Drawing.Size(173, 32);
            this.lblPreviewTitle.TabIndex = 0;
            this.lblPreviewTitle.Text = "Prévisualisation";
            // 
            // pnlSettings
            // 
            this.pnlSettings.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlSettings.Controls.Add(this.pnlSettingsScroll);
            this.pnlSettings.Controls.Add(this.pnlActionBar);
            this.pnlSettings.Controls.Add(this.pnlSidebarHeader);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSettings.Location = new System.Drawing.Point(790, 0);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(390, 760);
            this.pnlSettings.TabIndex = 1;
            // 
            // pnlSettingsScroll
            // 
            this.pnlSettingsScroll.AutoScroll = true;
            this.pnlSettingsScroll.AutoScrollMinSize = new System.Drawing.Size(0, 910);
            this.pnlSettingsScroll.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlSettingsScroll.Controls.Add(this.grpComp);
            this.pnlSettingsScroll.Controls.Add(this.grpFormat);
            this.pnlSettingsScroll.Controls.Add(this.grpCrop);
            this.pnlSettingsScroll.Controls.Add(this.grpDim);
            this.pnlSettingsScroll.Controls.Add(this.pnlFileInfo);
            this.pnlSettingsScroll.Controls.Add(this.btnOpen);
            this.pnlSettingsScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSettingsScroll.Location = new System.Drawing.Point(0, 92);
            this.pnlSettingsScroll.Name = "pnlSettingsScroll";
            this.pnlSettingsScroll.Size = new System.Drawing.Size(390, 550);
            this.pnlSettingsScroll.TabIndex = 1;
            // 
            // grpComp
            // 
            this.grpComp.BackColor = System.Drawing.Color.White;
            this.grpComp.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.grpComp.BorderSize = 1;
            this.grpComp.Controls.Add(this.lblCompHelp);
            this.grpComp.Controls.Add(this.lblCompTitle);
            this.grpComp.Controls.Add(this.lblQuality);
            this.grpComp.Controls.Add(this.trkQuality);
            this.grpComp.Controls.Add(this.chkTargetSize);
            this.grpComp.Controls.Add(this.numTargetKb);
            this.grpComp.Controls.Add(this.lblKb);
            this.grpComp.CornerRadius = 16;
            this.grpComp.Location = new System.Drawing.Point(20, 662);
            this.grpComp.Name = "grpComp";
            this.grpComp.Size = new System.Drawing.Size(348, 216);
            this.grpComp.TabIndex = 5;
            // 
            // lblCompHelp
            // 
            this.lblCompHelp.AutoSize = true;
            this.lblCompHelp.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCompHelp.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblCompHelp.Location = new System.Drawing.Point(19, 43);
            this.lblCompHelp.Name = "lblCompHelp";
            this.lblCompHelp.Size = new System.Drawing.Size(249, 15);
            this.lblCompHelp.TabIndex = 1;
            this.lblCompHelp.Text = "Ajuste la qualité ou impose un poids maximum.";
            // 
            // lblCompTitle
            // 
            this.lblCompTitle.AutoSize = true;
            this.lblCompTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblCompTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblCompTitle.Location = new System.Drawing.Point(18, 17);
            this.lblCompTitle.Name = "lblCompTitle";
            this.lblCompTitle.Size = new System.Drawing.Size(102, 20);
            this.lblCompTitle.TabIndex = 0;
            this.lblCompTitle.Text = "Compression";
            // 
            // lblQuality
            // 
            this.lblQuality.AutoSize = true;
            this.lblQuality.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblQuality.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblQuality.Location = new System.Drawing.Point(19, 70);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(67, 15);
            this.lblQuality.TabIndex = 2;
            this.lblQuality.Text = "Qualité : 85";
            // 
            // trkQuality
            // 
            this.trkQuality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkQuality.Location = new System.Drawing.Point(16, 87);
            this.trkQuality.Maximum = 100;
            this.trkQuality.Minimum = 1;
            this.trkQuality.Name = "trkQuality";
            this.trkQuality.Size = new System.Drawing.Size(314, 45);
            this.trkQuality.TabIndex = 3;
            this.trkQuality.TickFrequency = 10;
            this.trkQuality.Value = 85;
            // 
            // chkTargetSize
            // 
            this.chkTargetSize.AutoSize = true;
            this.chkTargetSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkTargetSize.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.chkTargetSize.Location = new System.Drawing.Point(22, 136);
            this.chkTargetSize.Name = "chkTargetSize";
            this.chkTargetSize.Size = new System.Drawing.Size(146, 19);
            this.chkTargetSize.TabIndex = 4;
            this.chkTargetSize.Text = "Cibler un poids maxi";
            this.chkTargetSize.UseVisualStyleBackColor = true;
            // 
            // numTargetKb
            // 
            this.numTargetKb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTargetKb.Enabled = false;
            this.numTargetKb.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numTargetKb.Location = new System.Drawing.Point(22, 169);
            this.numTargetKb.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numTargetKb.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTargetKb.Name = "numTargetKb";
            this.numTargetKb.Size = new System.Drawing.Size(132, 25);
            this.numTargetKb.TabIndex = 5;
            this.numTargetKb.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // lblKb
            // 
            this.lblKb.AutoSize = true;
            this.lblKb.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblKb.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblKb.Location = new System.Drawing.Point(161, 174);
            this.lblKb.Name = "lblKb";
            this.lblKb.Size = new System.Drawing.Size(22, 15);
            this.lblKb.TabIndex = 6;
            this.lblKb.Text = "Ko";
            // 
            // grpFormat
            // 
            this.grpFormat.BackColor = System.Drawing.Color.White;
            this.grpFormat.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.grpFormat.BorderSize = 1;
            this.grpFormat.Controls.Add(this.lblFormatHelp);
            this.grpFormat.Controls.Add(this.lblFormatTitle);
            this.grpFormat.Controls.Add(this.cmbFormat);
            this.grpFormat.CornerRadius = 16;
            this.grpFormat.Location = new System.Drawing.Point(20, 544);
            this.grpFormat.Name = "grpFormat";
            this.grpFormat.Size = new System.Drawing.Size(348, 106);
            this.grpFormat.TabIndex = 4;
            // 
            // lblFormatHelp
            // 
            this.lblFormatHelp.AutoSize = true;
            this.lblFormatHelp.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFormatHelp.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblFormatHelp.Location = new System.Drawing.Point(19, 42);
            this.lblFormatHelp.Name = "lblFormatHelp";
            this.lblFormatHelp.Size = new System.Drawing.Size(178, 15);
            this.lblFormatHelp.TabIndex = 1;
            this.lblFormatHelp.Text = "Choisis le format du fichier final.";
            // 
            // lblFormatTitle
            // 
            this.lblFormatTitle.AutoSize = true;
            this.lblFormatTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblFormatTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblFormatTitle.Location = new System.Drawing.Point(18, 16);
            this.lblFormatTitle.Name = "lblFormatTitle";
            this.lblFormatTitle.Size = new System.Drawing.Size(118, 20);
            this.lblFormatTitle.TabIndex = 0;
            this.lblFormatTitle.Text = "Format de sortie";
            // 
            // cmbFormat
            // 
            this.cmbFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFormat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.Items.AddRange(new object[] {
            "PNG",
            "JPEG",
            "WebP",
            "GIF"});
            this.cmbFormat.Location = new System.Drawing.Point(22, 67);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.Size = new System.Drawing.Size(304, 25);
            this.cmbFormat.TabIndex = 2;
            // 
            // grpCrop
            // 
            this.grpCrop.BackColor = System.Drawing.Color.White;
            this.grpCrop.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.grpCrop.BorderSize = 1;
            this.grpCrop.Controls.Add(this.lblCropHelp);
            this.grpCrop.Controls.Add(this.lblCropTitle);
            this.grpCrop.Controls.Add(this.rbNone);
            this.grpCrop.Controls.Add(this.rbCover);
            this.grpCrop.Controls.Add(this.rbContain);
            this.grpCrop.Controls.Add(this.rbManual);
            this.grpCrop.CornerRadius = 16;
            this.grpCrop.Location = new System.Drawing.Point(20, 336);
            this.grpCrop.Name = "grpCrop";
            this.grpCrop.Size = new System.Drawing.Size(348, 196);
            this.grpCrop.TabIndex = 3;
            // 
            // lblCropHelp
            // 
            this.lblCropHelp.AutoSize = true;
            this.lblCropHelp.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCropHelp.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblCropHelp.Location = new System.Drawing.Point(19, 43);
            this.lblCropHelp.Name = "lblCropHelp";
            this.lblCropHelp.Size = new System.Drawing.Size(257, 15);
            this.lblCropHelp.TabIndex = 1;
            this.lblCropHelp.Text = "Détermine la façon dont l’image remplit la zone.";
            // 
            // lblCropTitle
            // 
            this.lblCropTitle.AutoSize = true;
            this.lblCropTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblCropTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblCropTitle.Location = new System.Drawing.Point(18, 17);
            this.lblCropTitle.Name = "lblCropTitle";
            this.lblCropTitle.Size = new System.Drawing.Size(83, 20);
            this.lblCropTitle.TabIndex = 0;
            this.lblCropTitle.Text = "Recadrage";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbNone.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.rbNone.Location = new System.Drawing.Point(22, 72);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(159, 21);
            this.rbNone.TabIndex = 2;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "Aucun — garder l’image";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rbCover
            // 
            this.rbCover.AutoSize = true;
            this.rbCover.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbCover.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.rbCover.Location = new System.Drawing.Point(22, 101);
            this.rbCover.Name = "rbCover";
            this.rbCover.Size = new System.Drawing.Size(180, 21);
            this.rbCover.TabIndex = 3;
            this.rbCover.Text = "Cover — remplir et rogner";
            this.rbCover.UseVisualStyleBackColor = true;
            // 
            // rbContain
            // 
            this.rbContain.AutoSize = true;
            this.rbContain.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbContain.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.rbContain.Location = new System.Drawing.Point(22, 130);
            this.rbContain.Name = "rbContain";
            this.rbContain.Size = new System.Drawing.Size(199, 21);
            this.rbContain.TabIndex = 4;
            this.rbContain.Text = "Contain — ajuster avec marges";
            this.rbContain.UseVisualStyleBackColor = true;
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbManual.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.rbManual.Location = new System.Drawing.Point(22, 159);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(193, 21);
            this.rbManual.TabIndex = 5;
            this.rbManual.Text = "Manuel — sélectionner à la souris";
            this.rbManual.UseVisualStyleBackColor = true;
            // 
            // grpDim
            // 
            this.grpDim.BackColor = System.Drawing.Color.White;
            this.grpDim.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.grpDim.BorderSize = 1;
            this.grpDim.Controls.Add(this.lblDimHelp);
            this.grpDim.Controls.Add(this.lblDimTitle);
            this.grpDim.Controls.Add(this.lblW);
            this.grpDim.Controls.Add(this.numWidth);
            this.grpDim.Controls.Add(this.lblH);
            this.grpDim.Controls.Add(this.numHeight);
            this.grpDim.Controls.Add(this.chkKeepRatio);
            this.grpDim.CornerRadius = 16;
            this.grpDim.Location = new System.Drawing.Point(20, 154);
            this.grpDim.Name = "grpDim";
            this.grpDim.Size = new System.Drawing.Size(348, 170);
            this.grpDim.TabIndex = 2;
            // 
            // lblDimHelp
            // 
            this.lblDimHelp.AutoSize = true;
            this.lblDimHelp.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblDimHelp.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblDimHelp.Location = new System.Drawing.Point(19, 43);
            this.lblDimHelp.Name = "lblDimHelp";
            this.lblDimHelp.Size = new System.Drawing.Size(195, 15);
            this.lblDimHelp.TabIndex = 1;
            this.lblDimHelp.Text = "Définis la taille finale, exprimée en px.";
            // 
            // lblDimTitle
            // 
            this.lblDimTitle.AutoSize = true;
            this.lblDimTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblDimTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblDimTitle.Location = new System.Drawing.Point(18, 17);
            this.lblDimTitle.Name = "lblDimTitle";
            this.lblDimTitle.Size = new System.Drawing.Size(89, 20);
            this.lblDimTitle.TabIndex = 0;
            this.lblDimTitle.Text = "Dimensions";
            // 
            // lblW
            // 
            this.lblW.AutoSize = true;
            this.lblW.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblW.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblW.Location = new System.Drawing.Point(19, 78);
            this.lblW.Name = "lblW";
            this.lblW.Size = new System.Drawing.Size(49, 15);
            this.lblW.TabIndex = 2;
            this.lblW.Text = "Largeur";
            // 
            // numWidth
            // 
            this.numWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numWidth.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numWidth.Location = new System.Drawing.Point(190, 69);
            this.numWidth.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.numWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(136, 25);
            this.numWidth.TabIndex = 3;
            this.numWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numWidth.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // lblH
            // 
            this.lblH.AutoSize = true;
            this.lblH.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblH.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblH.Location = new System.Drawing.Point(19, 113);
            this.lblH.Name = "lblH";
            this.lblH.Size = new System.Drawing.Size(51, 15);
            this.lblH.TabIndex = 4;
            this.lblH.Text = "Hauteur";
            // 
            // numHeight
            // 
            this.numHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numHeight.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numHeight.Location = new System.Drawing.Point(190, 104);
            this.numHeight.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.numHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(136, 25);
            this.numHeight.TabIndex = 5;
            this.numHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numHeight.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // chkKeepRatio
            // 
            this.chkKeepRatio.AutoSize = true;
            this.chkKeepRatio.Checked = true;
            this.chkKeepRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeepRatio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkKeepRatio.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.chkKeepRatio.Location = new System.Drawing.Point(22, 141);
            this.chkKeepRatio.Name = "chkKeepRatio";
            this.chkKeepRatio.Size = new System.Drawing.Size(110, 19);
            this.chkKeepRatio.TabIndex = 6;
            this.chkKeepRatio.Text = "Garder le ratio";
            this.chkKeepRatio.UseVisualStyleBackColor = true;
            // 
            // pnlFileInfo
            // 
            this.pnlFileInfo.BackColor = System.Drawing.Color.White;
            this.pnlFileInfo.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlFileInfo.BorderSize = 1;
            this.pnlFileInfo.Controls.Add(this.pnlFileAccent);
            this.pnlFileInfo.Controls.Add(this.lblFileCaption);
            this.pnlFileInfo.Controls.Add(this.lblOriginal);
            this.pnlFileInfo.CornerRadius = 14;
            this.pnlFileInfo.Location = new System.Drawing.Point(20, 78);
            this.pnlFileInfo.Name = "pnlFileInfo";
            this.pnlFileInfo.Size = new System.Drawing.Size(348, 64);
            this.pnlFileInfo.TabIndex = 1;
            // 
            // pnlFileAccent
            // 
            this.pnlFileAccent.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.pnlFileAccent.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFileAccent.Location = new System.Drawing.Point(0, 0);
            this.pnlFileAccent.Name = "pnlFileAccent";
            this.pnlFileAccent.Size = new System.Drawing.Size(5, 64);
            this.pnlFileAccent.TabIndex = 0;
            // 
            // lblFileCaption
            // 
            this.lblFileCaption.AutoSize = true;
            this.lblFileCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.lblFileCaption.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.lblFileCaption.Location = new System.Drawing.Point(20, 10);
            this.lblFileCaption.Name = "lblFileCaption";
            this.lblFileCaption.Size = new System.Drawing.Size(82, 13);
            this.lblFileCaption.TabIndex = 1;
            this.lblFileCaption.Text = "IMAGE SOURCE";
            // 
            // lblOriginal
            // 
            this.lblOriginal.AutoEllipsis = true;
            this.lblOriginal.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblOriginal.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblOriginal.Location = new System.Drawing.Point(19, 30);
            this.lblOriginal.Name = "lblOriginal";
            this.lblOriginal.Size = new System.Drawing.Size(307, 22);
            this.lblOriginal.TabIndex = 2;
            this.lblOriginal.Text = "Aucune image chargée";
            this.lblOriginal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnOpen.CornerRadius = 13;
            this.btnOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpen.DisabledBackColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnOpen.DisabledTextColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnOpen.FlatAppearance.BorderSize = 0;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnOpen.ForeColor = System.Drawing.Color.White;
            this.btnOpen.HoverBackColor = System.Drawing.Color.FromArgb(29, 78, 216);
            this.btnOpen.Location = new System.Drawing.Point(20, 20);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.PressedBackColor = System.Drawing.Color.FromArgb(30, 64, 175);
            this.btnOpen.Size = new System.Drawing.Size(348, 46);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Ouvrir une image";
            this.btnOpen.UseVisualStyleBackColor = false;
            // 
            // pnlActionBar
            // 
            this.pnlActionBar.BackColor = System.Drawing.Color.White;
            this.pnlActionBar.Controls.Add(this.btnSave);
            this.pnlActionBar.Controls.Add(this.lblEstimated);
            this.pnlActionBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActionBar.Location = new System.Drawing.Point(0, 642);
            this.pnlActionBar.Name = "pnlActionBar";
            this.pnlActionBar.Padding = new System.Windows.Forms.Padding(20, 12, 20, 14);
            this.pnlActionBar.Size = new System.Drawing.Size(390, 118);
            this.pnlActionBar.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnSave.CornerRadius = 13;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSave.DisabledBackColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnSave.DisabledTextColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnSave.Enabled = false;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.HoverBackColor = System.Drawing.Color.FromArgb(21, 128, 61);
            this.btnSave.Location = new System.Drawing.Point(20, 60);
            this.btnSave.Name = "btnSave";
            this.btnSave.PressedBackColor = System.Drawing.Color.FromArgb(22, 101, 52);
            this.btnSave.Size = new System.Drawing.Size(350, 44);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Enregistrer l’image";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // lblEstimated
            // 
            this.lblEstimated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEstimated.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEstimated.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblEstimated.Location = new System.Drawing.Point(20, 13);
            this.lblEstimated.Name = "lblEstimated";
            this.lblEstimated.Size = new System.Drawing.Size(350, 38);
            this.lblEstimated.TabIndex = 0;
            this.lblEstimated.Text = "Poids estimé : —";
            this.lblEstimated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSidebarHeader
            // 
            this.pnlSidebarHeader.BackColor = System.Drawing.Color.White;
            this.pnlSidebarHeader.Controls.Add(this.pnlLogo);
            this.pnlSidebarHeader.Controls.Add(this.lblAppSubtitle);
            this.pnlSidebarHeader.Controls.Add(this.lblAppName);
            this.pnlSidebarHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebarHeader.Name = "pnlSidebarHeader";
            this.pnlSidebarHeader.Size = new System.Drawing.Size(390, 92);
            this.pnlSidebarHeader.TabIndex = 0;
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.pnlLogo.BorderColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.pnlLogo.BorderSize = 0;
            this.pnlLogo.Controls.Add(this.lblLogo);
            this.pnlLogo.CornerRadius = 12;
            this.pnlLogo.Location = new System.Drawing.Point(20, 23);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(44, 44);
            this.pnlLogo.TabIndex = 0;
            // 
            // lblLogo
            // 
            this.lblLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(44, 44);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "IE";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAppSubtitle
            // 
            this.lblAppSubtitle.AutoSize = true;
            this.lblAppSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblAppSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblAppSubtitle.Location = new System.Drawing.Point(76, 51);
            this.lblAppSubtitle.Name = "lblAppSubtitle";
            this.lblAppSubtitle.Size = new System.Drawing.Size(207, 15);
            this.lblAppSubtitle.TabIndex = 2;
            this.lblAppSubtitle.Text = "Avatars, bannières et images pour le web";
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblAppName.Location = new System.Drawing.Point(73, 19);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(134, 30);
            this.lblAppName.TabIndex = 1;
            this.lblAppName.Text = "ImageEditor";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.ClientSize = new System.Drawing.Size(1180, 760);
            this.Controls.Add(this.pnlPreviewArea);
            this.Controls.Add(this.pnlSettings);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(980, 650);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageEditor — Avatar & Bannière";
            this.pnlPreviewArea.ResumeLayout(false);
            this.pnlPreviewPadding.ResumeLayout(false);
            this.pnlPreviewCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.pnlPreviewFooter.ResumeLayout(false);
            this.pnlPreviewHeader.ResumeLayout(false);
            this.pnlPreviewHeader.PerformLayout();
            this.pnlSettings.ResumeLayout(false);
            this.pnlSettingsScroll.ResumeLayout(false);
            this.grpComp.ResumeLayout(false);
            this.grpComp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkQuality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetKb)).EndInit();
            this.grpFormat.ResumeLayout(false);
            this.grpFormat.PerformLayout();
            this.grpCrop.ResumeLayout(false);
            this.grpCrop.PerformLayout();
            this.grpDim.ResumeLayout(false);
            this.grpDim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.pnlFileInfo.ResumeLayout(false);
            this.pnlFileInfo.PerformLayout();
            this.pnlActionBar.ResumeLayout(false);
            this.pnlSidebarHeader.ResumeLayout(false);
            this.pnlSidebarHeader.PerformLayout();
            this.pnlLogo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlPreviewArea;
        private System.Windows.Forms.Panel pnlPreviewPadding;
        private ImageEditor.RoundedPanel pnlPreviewCard;
        private ImageEditor.PreviewPictureBox picPreview;
        private System.Windows.Forms.Panel pnlPreviewFooter;
        private System.Windows.Forms.Label lblDropHint;
        private System.Windows.Forms.Panel pnlPreviewHeader;
        private System.Windows.Forms.Label lblPreviewSubtitle;
        private System.Windows.Forms.Label lblPreviewTitle;
        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.Panel pnlSettingsScroll;
        private ImageEditor.RoundedPanel grpComp;
        private System.Windows.Forms.Label lblCompHelp;
        private System.Windows.Forms.Label lblCompTitle;
        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.TrackBar trkQuality;
        private System.Windows.Forms.CheckBox chkTargetSize;
        private System.Windows.Forms.NumericUpDown numTargetKb;
        private System.Windows.Forms.Label lblKb;
        private ImageEditor.RoundedPanel grpFormat;
        private System.Windows.Forms.Label lblFormatHelp;
        private System.Windows.Forms.Label lblFormatTitle;
        private System.Windows.Forms.ComboBox cmbFormat;
        private ImageEditor.RoundedPanel grpCrop;
        private System.Windows.Forms.Label lblCropHelp;
        private System.Windows.Forms.Label lblCropTitle;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbCover;
        private System.Windows.Forms.RadioButton rbContain;
        private System.Windows.Forms.RadioButton rbManual;
        private ImageEditor.RoundedPanel grpDim;
        private System.Windows.Forms.Label lblDimHelp;
        private System.Windows.Forms.Label lblDimTitle;
        private System.Windows.Forms.Label lblW;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label lblH;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.CheckBox chkKeepRatio;
        private ImageEditor.RoundedPanel pnlFileInfo;
        private System.Windows.Forms.Panel pnlFileAccent;
        private System.Windows.Forms.Label lblFileCaption;
        private System.Windows.Forms.Label lblOriginal;
        private ImageEditor.ModernButton btnOpen;
        private System.Windows.Forms.Panel pnlActionBar;
        private ImageEditor.ModernButton btnSave;
        private System.Windows.Forms.Label lblEstimated;
        private System.Windows.Forms.Panel pnlSidebarHeader;
        private ImageEditor.RoundedPanel pnlLogo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblAppSubtitle;
        private System.Windows.Forms.Label lblAppName;
    }
}
