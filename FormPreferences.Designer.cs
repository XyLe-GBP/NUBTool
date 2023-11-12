namespace NUBTool
{
    partial class FormPreferences
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
            button_Cancel = new Button();
            button_OK = new Button();
            checkBox_showdirectory = new CheckBox();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button_Cancel
            // 
            button_Cancel.Location = new Point(272, 173);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new Size(75, 23);
            button_Cancel.TabIndex = 0;
            button_Cancel.Text = "Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_OK
            // 
            button_OK.Location = new Point(191, 173);
            button_OK.Name = "button_OK";
            button_OK.Size = new Size(75, 23);
            button_OK.TabIndex = 1;
            button_OK.Text = "OK";
            button_OK.UseVisualStyleBackColor = true;
            // 
            // checkBox_showdirectory
            // 
            checkBox_showdirectory.AutoSize = true;
            checkBox_showdirectory.Location = new Point(3, 145);
            checkBox_showdirectory.Name = "checkBox_showdirectory";
            checkBox_showdirectory.Size = new Size(350, 19);
            checkBox_showdirectory.TabIndex = 2;
            checkBox_showdirectory.Text = "Display destination directory when file conversion is complete";
            checkBox_showdirectory.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(checkBox_showdirectory);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(359, 167);
            panel1.TabIndex = 3;
            // 
            // FormPreferences
            // 
            AcceptButton = button_OK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = button_Cancel;
            ClientSize = new Size(359, 206);
            ControlBox = false;
            Controls.Add(panel1);
            Controls.Add(button_OK);
            Controls.Add(button_Cancel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FormPreferences";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Preferences";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button_Cancel;
        private Button button_OK;
        private CheckBox checkBox_showdirectory;
        private Panel panel1;
    }
}