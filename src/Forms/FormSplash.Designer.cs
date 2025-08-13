namespace NUBTool
{
    partial class FormSplash
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            progressBar1 = new ProgressBar();
            label_log = new Label();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(12, 146);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(276, 10);
            progressBar1.TabIndex = 0;
            // 
            // label_log
            // 
            label_log.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label_log.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label_log.Location = new Point(12, 120);
            label_log.Name = "label_log";
            label_log.Size = new Size(276, 23);
            label_log.TabIndex = 1;
            label_log.Text = "Loading...";
            label_log.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormSplash
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(300, 164);
            ControlBox = false;
            Controls.Add(label_log);
            Controls.Add(progressBar1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormSplash";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormSplash";
            Load += FormSplash_Load;
            ResumeLayout(false);
        }

        #endregion

        private ProgressBar progressBar1;
        internal Label label_log;
    }
}