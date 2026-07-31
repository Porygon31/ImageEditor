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
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lblOriginal = new System.Windows.Forms.Label();
            this.grpDim = new System.Windows.Forms.GroupBox();
            this.lblW = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.lblH = new System.Windows.Forms.Label();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.chkKeepRatio = new System.Windows.Forms.CheckBox();
            this.grpCrop = new System.Windows.Forms.GroupBox();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbCover = new System.Windows.Forms.RadioButton();
            this.rbContain = new System.Windows.Forms.RadioButton();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.grpFormat = new System.Windows.Forms.GroupBox();
            this.cmbFormat = new System.Windows.Forms.ComboBox();
            this.grpComp = new System.Windows.Forms.GroupBox();
            this.lblQuality = new System.Windows.Forms.Label();
            this.trkQuality = new System.Windows.Forms.TrackBar();
            this.chkTargetSize = new System.Windows.Forms.CheckBox();
            this.numTargetKb = new System.Windows.Forms.NumericUpDown();
            this.lblKb = new System.Windows.Forms.Label();
            this.lblEstimated = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.pnlSettings.SuspendLayout();
            this.grpDim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.grpCrop.SuspendLayout();
            this.grpFormat.SuspendLayout();
            this.grpComp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkQuality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetKb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            //
            // pnlSettings
            //
            this.pnlSettings.AutoScroll = true;
            this.pnlSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSettings.Controls.Add(this.btnOpen);
            this.pnlSettings.Controls.Add(this.lblOriginal);
            this.pnlSettings.Controls.Add(this.grpDim);
            this.pnlSettings.Controls.Add(this.grpCrop);
            this.pnlSettings.Controls.Add(this.grpFormat);
            this.pnlSettings.Controls.Add(this.grpComp);
            this.pnlSettings.Controls.Add(this.lblEstimated);
            this.pnlSettings.Controls.Add(this.btnSave);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSettings.Location = new System.Drawing.Point(670, 0);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(290, 641);
            this.pnlSettings.TabIndex = 1;
            //
            // btnOpen
            //
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(265, 32);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Ouvrir une image…";
            this.btnOpen.UseVisualStyleBackColor = true;
            //
            // lblOriginal
            //
            this.lblOriginal.AutoSize = true;
            this.lblOriginal.Location = new System.Drawing.Point(12, 52);
            this.lblOriginal.Name = "lblOriginal";
            this.lblOriginal.Size = new System.Drawing.Size(80, 13);
            this.lblOriginal.TabIndex = 1;
            this.lblOriginal.Text = "Aucune image";
            //
            // grpDim
            //
            this.grpDim.Controls.Add(this.lblW);
            this.grpDim.Controls.Add(this.numWidth);
            this.grpDim.Controls.Add(this.lblH);
            this.grpDim.Controls.Add(this.numHeight);
            this.grpDim.Controls.Add(this.chkKeepRatio);
            this.grpDim.Location = new System.Drawing.Point(12, 78);
            this.grpDim.Name = "grpDim";
            this.grpDim.Size = new System.Drawing.Size(265, 112);
            this.grpDim.TabIndex = 2;
            this.grpDim.TabStop = false;
            this.grpDim.Text = "Dimensions (px)";
            //
            // lblW
            //
            this.lblW.AutoSize = true;
            this.lblW.Location = new System.Drawing.Point(12, 26);
            this.lblW.Name = "lblW";
            this.lblW.Size = new System.Drawing.Size(46, 13);
            this.lblW.TabIndex = 0;
            this.lblW.Text = "Largeur";
            //
            // numWidth
            //
            this.numWidth.Location = new System.Drawing.Point(90, 24);
            this.numWidth.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            this.numWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(90, 20);
            this.numWidth.TabIndex = 1;
            this.numWidth.Value = new decimal(new int[] { 512, 0, 0, 0 });
            //
            // lblH
            //
            this.lblH.AutoSize = true;
            this.lblH.Location = new System.Drawing.Point(12, 54);
            this.lblH.Name = "lblH";
            this.lblH.Size = new System.Drawing.Size(49, 13);
            this.lblH.TabIndex = 2;
            this.lblH.Text = "Hauteur";
            //
            // numHeight
            //
            this.numHeight.Location = new System.Drawing.Point(90, 52);
            this.numHeight.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            this.numHeight.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(90, 20);
            this.numHeight.TabIndex = 3;
            this.numHeight.Value = new decimal(new int[] { 512, 0, 0, 0 });
            //
            // chkKeepRatio
            //
            this.chkKeepRatio.AutoSize = true;
            this.chkKeepRatio.Checked = true;
            this.chkKeepRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeepRatio.Location = new System.Drawing.Point(15, 84);
            this.chkKeepRatio.Name = "chkKeepRatio";
            this.chkKeepRatio.Size = new System.Drawing.Size(98, 17);
            this.chkKeepRatio.TabIndex = 4;
            this.chkKeepRatio.Text = "Garder le ratio";
            this.chkKeepRatio.UseVisualStyleBackColor = true;
            //
            // grpCrop
            //
            this.grpCrop.Controls.Add(this.rbNone);
            this.grpCrop.Controls.Add(this.rbCover);
            this.grpCrop.Controls.Add(this.rbContain);
            this.grpCrop.Controls.Add(this.rbManual);
            this.grpCrop.Location = new System.Drawing.Point(12, 196);
            this.grpCrop.Name = "grpCrop";
            this.grpCrop.Size = new System.Drawing.Size(265, 128);
            this.grpCrop.TabIndex = 3;
            this.grpCrop.TabStop = false;
            this.grpCrop.Text = "Recadrage";
            //
            // rbNone
            //
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.Location = new System.Drawing.Point(15, 24);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(53, 17);
            this.rbNone.TabIndex = 0;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "Aucun";
            this.rbNone.UseVisualStyleBackColor = true;
            //
            // rbCover
            //
            this.rbCover.AutoSize = true;
            this.rbCover.Location = new System.Drawing.Point(15, 48);
            this.rbCover.Name = "rbCover";
            this.rbCover.Size = new System.Drawing.Size(160, 17);
            this.rbCover.TabIndex = 1;
            this.rbCover.Text = "Cover (remplir + rogner)";
            this.rbCover.UseVisualStyleBackColor = true;
            //
            // rbContain
            //
            this.rbContain.AutoSize = true;
            this.rbContain.Location = new System.Drawing.Point(15, 72);
            this.rbContain.Name = "rbContain";
            this.rbContain.Size = new System.Drawing.Size(168, 17);
            this.rbContain.TabIndex = 2;
            this.rbContain.Text = "Contain (ajuster + marges)";
            this.rbContain.UseVisualStyleBackColor = true;
            //
            // rbManual
            //
            this.rbManual.AutoSize = true;
            this.rbManual.Location = new System.Drawing.Point(15, 96);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(120, 17);
            this.rbManual.TabIndex = 3;
            this.rbManual.Text = "Manuel (souris)";
            this.rbManual.UseVisualStyleBackColor = true;
            //
            // grpFormat
            //
            this.grpFormat.Controls.Add(this.cmbFormat);
            this.grpFormat.Location = new System.Drawing.Point(12, 330);
            this.grpFormat.Name = "grpFormat";
            this.grpFormat.Size = new System.Drawing.Size(265, 60);
            this.grpFormat.TabIndex = 4;
            this.grpFormat.TabStop = false;
            this.grpFormat.Text = "Format de sortie";
            //
            // cmbFormat
            //
            this.cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.Items.AddRange(new object[] { "PNG", "JPEG", "WebP", "GIF" });
            this.cmbFormat.Location = new System.Drawing.Point(12, 24);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.Size = new System.Drawing.Size(241, 21);
            this.cmbFormat.TabIndex = 0;
            //
            // grpComp
            //
            this.grpComp.Controls.Add(this.lblQuality);
            this.grpComp.Controls.Add(this.trkQuality);
            this.grpComp.Controls.Add(this.chkTargetSize);
            this.grpComp.Controls.Add(this.numTargetKb);
            this.grpComp.Controls.Add(this.lblKb);
            this.grpComp.Location = new System.Drawing.Point(12, 396);
            this.grpComp.Name = "grpComp";
            this.grpComp.Size = new System.Drawing.Size(265, 150);
            this.grpComp.TabIndex = 5;
            this.grpComp.TabStop = false;
            this.grpComp.Text = "Compression";
            //
            // lblQuality
            //
            this.lblQuality.AutoSize = true;
            this.lblQuality.Location = new System.Drawing.Point(12, 22);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(66, 13);
            this.lblQuality.TabIndex = 0;
            this.lblQuality.Text = "Qualité : 85";
            //
            // trkQuality
            //
            this.trkQuality.Location = new System.Drawing.Point(9, 40);
            this.trkQuality.Maximum = 100;
            this.trkQuality.Minimum = 1;
            this.trkQuality.Name = "trkQuality";
            this.trkQuality.Size = new System.Drawing.Size(244, 45);
            this.trkQuality.TabIndex = 1;
            this.trkQuality.TickFrequency = 10;
            this.trkQuality.Value = 85;
            //
            // chkTargetSize
            //
            this.chkTargetSize.AutoSize = true;
            this.chkTargetSize.Location = new System.Drawing.Point(15, 92);
            this.chkTargetSize.Name = "chkTargetSize";
            this.chkTargetSize.Size = new System.Drawing.Size(140, 17);
            this.chkTargetSize.TabIndex = 2;
            this.chkTargetSize.Text = "Cibler un poids maxi";
            this.chkTargetSize.UseVisualStyleBackColor = true;
            //
            // numTargetKb
            //
            this.numTargetKb.Enabled = false;
            this.numTargetKb.Location = new System.Drawing.Point(15, 116);
            this.numTargetKb.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numTargetKb.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numTargetKb.Name = "numTargetKb";
            this.numTargetKb.Size = new System.Drawing.Size(120, 20);
            this.numTargetKb.TabIndex = 3;
            this.numTargetKb.Value = new decimal(new int[] { 256, 0, 0, 0 });
            //
            // lblKb
            //
            this.lblKb.AutoSize = true;
            this.lblKb.Location = new System.Drawing.Point(141, 118);
            this.lblKb.Name = "lblKb";
            this.lblKb.Size = new System.Drawing.Size(20, 13);
            this.lblKb.TabIndex = 4;
            this.lblKb.Text = "Ko";
            //
            // lblEstimated
            //
            this.lblEstimated.AutoSize = true;
            this.lblEstimated.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstimated.Location = new System.Drawing.Point(12, 556);
            this.lblEstimated.Name = "lblEstimated";
            this.lblEstimated.Size = new System.Drawing.Size(105, 15);
            this.lblEstimated.TabIndex = 6;
            this.lblEstimated.Text = "Poids estimé : —";
            //
            // btnSave
            //
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(12, 582);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(265, 36);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Enregistrer…";
            this.btnSave.UseVisualStyleBackColor = true;
            //
            // picPreview
            //
            this.picPreview.BackColor = System.Drawing.SystemColors.ControlDark;
            this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPreview.Location = new System.Drawing.Point(0, 0);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(670, 641);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;
            //
            // MainForm
            //
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 641);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.pnlSettings);
            this.MinimumSize = new System.Drawing.Size(760, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageEditor — Avatar & Bannière";
            this.pnlSettings.ResumeLayout(false);
            this.pnlSettings.PerformLayout();
            this.grpDim.ResumeLayout(false);
            this.grpDim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.grpCrop.ResumeLayout(false);
            this.grpCrop.PerformLayout();
            this.grpFormat.ResumeLayout(false);
            this.grpComp.ResumeLayout(false);
            this.grpComp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkQuality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetKb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label lblOriginal;
        private System.Windows.Forms.GroupBox grpDim;
        private System.Windows.Forms.Label lblW;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label lblH;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.CheckBox chkKeepRatio;
        private System.Windows.Forms.GroupBox grpCrop;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbCover;
        private System.Windows.Forms.RadioButton rbContain;
        private System.Windows.Forms.RadioButton rbManual;
        private System.Windows.Forms.GroupBox grpFormat;
        private System.Windows.Forms.ComboBox cmbFormat;
        private System.Windows.Forms.GroupBox grpComp;
        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.TrackBar trkQuality;
        private System.Windows.Forms.CheckBox chkTargetSize;
        private System.Windows.Forms.NumericUpDown numTargetKb;
        private System.Windows.Forms.Label lblKb;
        private System.Windows.Forms.Label lblEstimated;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox picPreview;
    }
}
