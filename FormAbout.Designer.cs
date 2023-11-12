namespace NUBTool
{
    partial class FormAbout
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
            label1 = new Label();
            pictureBox_Icon = new PictureBox();
            linkLabel_github = new LinkLabel();
            linkLabel_web = new LinkLabel();
            label2 = new Label();
            label3 = new Label();
            button_OK = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Icon).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(8, 9);
            label1.Name = "label1";
            label1.Size = new Size(403, 30);
            label1.TabIndex = 0;
            label1.Text = "NUBTool - Namco nuSound Archive Utility";
            // 
            // pictureBox_Icon
            // 
            pictureBox_Icon.Location = new Point(24, 188);
            pictureBox_Icon.Name = "pictureBox_Icon";
            pictureBox_Icon.Size = new Size(128, 128);
            pictureBox_Icon.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_Icon.TabIndex = 1;
            pictureBox_Icon.TabStop = false;
            pictureBox_Icon.Paint += pictureBox_Icon_Paint;
            // 
            // linkLabel_github
            // 
            linkLabel_github.AutoSize = true;
            linkLabel_github.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            linkLabel_github.Location = new Point(240, 205);
            linkLabel_github.Name = "linkLabel_github";
            linkLabel_github.Size = new Size(107, 21);
            linkLabel_github.TabIndex = 2;
            linkLabel_github.TabStop = true;
            linkLabel_github.Text = "XyLe's GitHub";
            linkLabel_github.LinkClicked += linkLabel_github_LinkClicked;
            // 
            // linkLabel_web
            // 
            linkLabel_web.AutoSize = true;
            linkLabel_web.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            linkLabel_web.Location = new Point(210, 269);
            linkLabel_web.Name = "linkLabel_web";
            linkLabel_web.Size = new Size(165, 21);
            linkLabel_web.TabIndex = 3;
            linkLabel_web.TabStop = true;
            linkLabel_web.Text = "XyLe's Official Website";
            linkLabel_web.LinkClicked += linkLabel_web_LinkClicked;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(90, 147);
            label2.Name = "label2";
            label2.Size = new Size(245, 15);
            label2.TabIndex = 4;
            label2.Text = "Copyright © 2023 - XyLe. All Rights Reserved.";
            // 
            // label3
            // 
            label3.Location = new Point(8, 54);
            label3.Name = "label3";
            label3.Size = new Size(403, 75);
            label3.TabIndex = 5;
            label3.Text = "This application uses the SoX library for speech conversion.\n\nThe creator assumes no responsibility for any damage caused by the use of this application.";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button_OK
            // 
            button_OK.Location = new Point(332, 321);
            button_OK.Name = "button_OK";
            button_OK.Size = new Size(75, 23);
            button_OK.TabIndex = 6;
            button_OK.Text = "Done!";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += button_OK_Click;
            // 
            // FormAbout
            // 
            AcceptButton = button_OK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(419, 356);
            ControlBox = false;
            Controls.Add(button_OK);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(linkLabel_web);
            Controls.Add(linkLabel_github);
            Controls.Add(pictureBox_Icon);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FormAbout";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "About NUBTool";
            Load += FormAbout_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_Icon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox pictureBox_Icon;
        private LinkLabel linkLabel_github;
        private LinkLabel linkLabel_web;
        private Label label2;
        private Label label3;
        private Button button_OK;
    }
}