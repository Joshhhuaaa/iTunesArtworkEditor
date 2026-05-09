namespace iTunesArtworkEditor
{
    partial class UseLatestProgressForm
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
            this.label_Heading = new System.Windows.Forms.Label();
            this.label_Current = new System.Windows.Forms.Label();
            this.progressBar_Main = new System.Windows.Forms.ProgressBar();
            this.label_Count = new System.Windows.Forms.Label();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label_Heading
            //
            this.label_Heading.AutoSize = true;
            this.label_Heading.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label_Heading.Location = new System.Drawing.Point(15, 15);
            this.label_Heading.Name = "label_Heading";
            this.label_Heading.Size = new System.Drawing.Size(240, 15);
            this.label_Heading.TabIndex = 0;
            this.label_Heading.Text = "Updating artist artwork from Apple Music...";
            //
            // label_Current
            //
            this.label_Current.AutoSize = false;
            this.label_Current.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Current.Location = new System.Drawing.Point(15, 38);
            this.label_Current.Name = "label_Current";
            this.label_Current.Size = new System.Drawing.Size(450, 17);
            this.label_Current.TabIndex = 1;
            this.label_Current.Text = "";
            //
            // progressBar_Main
            //
            this.progressBar_Main.Location = new System.Drawing.Point(15, 62);
            this.progressBar_Main.Name = "progressBar_Main";
            this.progressBar_Main.Size = new System.Drawing.Size(450, 23);
            this.progressBar_Main.TabIndex = 2;
            //
            // label_Count
            //
            this.label_Count.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Count.AutoSize = true;
            this.label_Count.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Count.Location = new System.Drawing.Point(420, 92);
            this.label_Count.Name = "label_Count";
            this.label_Count.Size = new System.Drawing.Size(30, 15);
            this.label_Count.TabIndex = 3;
            this.label_Count.Text = "0 / 0";
            this.label_Count.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // button_Cancel
            //
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.Location = new System.Drawing.Point(385, 122);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(80, 27);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            //
            // UseLatestProgressForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 164);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.label_Count);
            this.Controls.Add(this.progressBar_Main);
            this.Controls.Add(this.label_Current);
            this.Controls.Add(this.label_Heading);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UseLatestProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Updating Artist Artwork";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label_Heading;
        private System.Windows.Forms.Label label_Current;
        private System.Windows.Forms.ProgressBar progressBar_Main;
        private System.Windows.Forms.Label label_Count;
        private System.Windows.Forms.Button button_Cancel;
    }
}
