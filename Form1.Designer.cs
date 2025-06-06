namespace Kaenen
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnWczytaj;
        private System.Windows.Forms.Button btnOblicz;
        private System.Windows.Forms.ComboBox cmbMetryki;
        private System.Windows.Forms.TextBox txtWyniki;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblMetryka;
        private System.Windows.Forms.Label lblWyniki;
        private System.Windows.Forms.NumericUpDown nudK;
        private System.Windows.Forms.Label lblK;
        private System.Windows.Forms.Button btnNormalizuj;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnWczytaj = new System.Windows.Forms.Button();
            this.btnOblicz = new System.Windows.Forms.Button();
            this.cmbMetryki = new System.Windows.Forms.ComboBox();
            this.txtWyniki = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblMetryka = new System.Windows.Forms.Label();
            this.lblWyniki = new System.Windows.Forms.Label();
            this.nudK = new System.Windows.Forms.NumericUpDown();
            this.lblK = new System.Windows.Forms.Label();
            this.btnNormalizuj = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudK)).BeginInit();
            this.SuspendLayout();
            
            // btnWczytaj
            this.btnWczytaj.Location = new System.Drawing.Point(12, 12);
            this.btnWczytaj.Name = "btnWczytaj";
            this.btnWczytaj.Size = new System.Drawing.Size(100, 30);
            this.btnWczytaj.TabIndex = 0;
            this.btnWczytaj.Text = "Wczytaj dane";
            this.btnWczytaj.UseVisualStyleBackColor = true;
            
            // btnNormalizuj
            this.btnNormalizuj.Enabled = false;
            this.btnNormalizuj.Location = new System.Drawing.Point(118, 12);
            this.btnNormalizuj.Name = "btnNormalizuj";
            this.btnNormalizuj.Size = new System.Drawing.Size(100, 30);
            this.btnNormalizuj.TabIndex = 1;
            this.btnNormalizuj.Text = "Normalizuj dane";
            this.btnNormalizuj.UseVisualStyleBackColor = true;
            
            // cmbMetryki
            this.cmbMetryki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMetryki.FormattingEnabled = true;
            this.cmbMetryki.Location = new System.Drawing.Point(70, 52);
            this.cmbMetryki.Name = "cmbMetryki";
            this.cmbMetryki.Size = new System.Drawing.Size(150, 21);
            this.cmbMetryki.TabIndex = 2;
            
            // btnOblicz
            this.btnOblicz.Enabled = false;
            this.btnOblicz.Location = new System.Drawing.Point(380, 50);
            this.btnOblicz.Name = "btnOblicz";
            this.btnOblicz.Size = new System.Drawing.Size(100, 25);
            this.btnOblicz.TabIndex = 3;
            this.btnOblicz.Text = "Oblicz";
            this.btnOblicz.UseVisualStyleBackColor = true;
            
            // txtWyniki
            this.txtWyniki.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtWyniki.Location = new System.Drawing.Point(12, 105);
            this.txtWyniki.Multiline = true;
            this.txtWyniki.Name = "txtWyniki";
            this.txtWyniki.ReadOnly = true;
            this.txtWyniki.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtWyniki.Size = new System.Drawing.Size(560, 300);
            this.txtWyniki.TabIndex = 4;
            
            // Etykiety
            this.lblMetryka.AutoSize = true;
            this.lblMetryka.Location = new System.Drawing.Point(12, 55);
            this.lblMetryka.Name = "lblMetryka";
            this.lblMetryka.Size = new System.Drawing.Size(50, 13);
            this.lblMetryka.TabIndex = 5;
            this.lblMetryka.Text = "Metryka:";
            
            this.lblK.AutoSize = true;
            this.lblK.Location = new System.Drawing.Point(240, 55);
            this.lblK.Name = "lblK";
            this.lblK.Size = new System.Drawing.Size(17, 13);
            this.lblK.TabIndex = 6;
            this.lblK.Text = "k:";
            
            this.lblWyniki.AutoSize = true;
            this.lblWyniki.Location = new System.Drawing.Point(12, 85);
            this.lblWyniki.Name = "lblWyniki";
            this.lblWyniki.Size = new System.Drawing.Size(40, 13);
            this.lblWyniki.TabIndex = 7;
            this.lblWyniki.Text = "Wyniki:";
            
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 420);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 13);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Nie wczytano danych";
            
            // nudK
            this.nudK.Location = new System.Drawing.Point(260, 52);
            this.nudK.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudK.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.nudK.Name = "nudK";
            this.nudK.Size = new System.Drawing.Size(50, 20);
            this.nudK.TabIndex = 9;
            this.nudK.Value = new decimal(new int[] { 3, 0, 0, 0 });
            
            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 450);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.nudK);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblWyniki);
            this.Controls.Add(this.lblK);
            this.Controls.Add(this.lblMetryka);
            this.Controls.Add(this.txtWyniki);
            this.Controls.Add(this.btnOblicz);
            this.Controls.Add(this.cmbMetryki);
            this.Controls.Add(this.btnNormalizuj);
            this.Controls.Add(this.btnWczytaj);
            this.Name = "Form1";
            this.Text = "Kaenen - Klasyfikator k-NN";
            ((System.ComponentModel.ISupportInitialize)(this.nudK)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}