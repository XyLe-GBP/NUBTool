namespace NUBTool
{
    partial class FormUpdateApplicationType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdateApplicationType));
            comboBox_type = new ComboBox();
            button_OK = new Button();
            button_Cancel = new Button();
            SuspendLayout();
            // 
            // comboBox_type
            // 
            resources.ApplyResources(comboBox_type, "comboBox_type");
            comboBox_type.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_type.FormattingEnabled = true;
            comboBox_type.Items.AddRange(new object[] { resources.GetString("comboBox_type.Items"), resources.GetString("comboBox_type.Items1") });
            comboBox_type.Name = "comboBox_type";
            // 
            // button_OK
            // 
            resources.ApplyResources(button_OK, "button_OK");
            button_OK.Name = "button_OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += Button_OK_Click;
            // 
            // button_Cancel
            // 
            resources.ApplyResources(button_Cancel, "button_Cancel");
            button_Cancel.Name = "button_Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += Button_Cancel_Click;
            // 
            // FormUpdateApplicationType
            // 
            AcceptButton = button_OK;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = button_Cancel;
            ControlBox = false;
            Controls.Add(button_Cancel);
            Controls.Add(button_OK);
            Controls.Add(comboBox_type);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FormUpdateApplicationType";
            Load += FormUpdateApplicationType_Load;
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboBox_type;
        private Button button_OK;
        private Button button_Cancel;
    }
}