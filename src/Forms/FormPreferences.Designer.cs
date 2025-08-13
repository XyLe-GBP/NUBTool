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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreferences));
            button_Cancel = new Button();
            button_OK = new Button();
            checkBox_showdirectory = new CheckBox();
            checkBox_Updates = new CheckBox();
            checkBox_SmoothSamples = new CheckBox();
            checkBox_ATWFix = new CheckBox();
            comboBox_ATWFix = new ComboBox();
            checkBox_ForceWaveConvertion = new CheckBox();
            tabControl_Main = new TabControl();
            tabPage1 = new TabPage();
            button_SplashImage = new Button();
            textBox_SplashImage = new TextBox();
            checkBox_SplashImage = new CheckBox();
            checkBox_HideSplash = new CheckBox();
            tabPage2 = new TabPage();
            checkBox_LoopWarning = new CheckBox();
            panel1 = new Panel();
            tabControl_Main.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button_Cancel
            // 
            resources.ApplyResources(button_Cancel, "button_Cancel");
            button_Cancel.Name = "button_Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += Button_Cancel_Click;
            // 
            // button_OK
            // 
            resources.ApplyResources(button_OK, "button_OK");
            button_OK.Name = "button_OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += Button_OK_Click;
            // 
            // checkBox_showdirectory
            // 
            resources.ApplyResources(checkBox_showdirectory, "checkBox_showdirectory");
            checkBox_showdirectory.Name = "checkBox_showdirectory";
            checkBox_showdirectory.UseVisualStyleBackColor = true;
            // 
            // checkBox_Updates
            // 
            resources.ApplyResources(checkBox_Updates, "checkBox_Updates");
            checkBox_Updates.Checked = true;
            checkBox_Updates.CheckState = CheckState.Checked;
            checkBox_Updates.Name = "checkBox_Updates";
            checkBox_Updates.UseVisualStyleBackColor = true;
            // 
            // checkBox_SmoothSamples
            // 
            resources.ApplyResources(checkBox_SmoothSamples, "checkBox_SmoothSamples");
            checkBox_SmoothSamples.Checked = true;
            checkBox_SmoothSamples.CheckState = CheckState.Checked;
            checkBox_SmoothSamples.Name = "checkBox_SmoothSamples";
            checkBox_SmoothSamples.UseVisualStyleBackColor = true;
            // 
            // checkBox_ATWFix
            // 
            resources.ApplyResources(checkBox_ATWFix, "checkBox_ATWFix");
            checkBox_ATWFix.Name = "checkBox_ATWFix";
            checkBox_ATWFix.UseVisualStyleBackColor = true;
            checkBox_ATWFix.CheckedChanged += CheckBox_ATWFix_CheckedChanged;
            // 
            // comboBox_ATWFix
            // 
            resources.ApplyResources(comboBox_ATWFix, "comboBox_ATWFix");
            comboBox_ATWFix.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_ATWFix.FormattingEnabled = true;
            comboBox_ATWFix.Items.AddRange(new object[] { resources.GetString("comboBox_ATWFix.Items"), resources.GetString("comboBox_ATWFix.Items1") });
            comboBox_ATWFix.Name = "comboBox_ATWFix";
            // 
            // checkBox_ForceWaveConvertion
            // 
            resources.ApplyResources(checkBox_ForceWaveConvertion, "checkBox_ForceWaveConvertion");
            checkBox_ForceWaveConvertion.Name = "checkBox_ForceWaveConvertion";
            checkBox_ForceWaveConvertion.UseVisualStyleBackColor = true;
            // 
            // tabControl_Main
            // 
            resources.ApplyResources(tabControl_Main, "tabControl_Main");
            tabControl_Main.Controls.Add(tabPage1);
            tabControl_Main.Controls.Add(tabPage2);
            tabControl_Main.Name = "tabControl_Main";
            tabControl_Main.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Controls.Add(button_SplashImage);
            tabPage1.Controls.Add(textBox_SplashImage);
            tabPage1.Controls.Add(checkBox_SplashImage);
            tabPage1.Controls.Add(checkBox_HideSplash);
            tabPage1.Controls.Add(checkBox_Updates);
            tabPage1.Controls.Add(checkBox_SmoothSamples);
            tabPage1.Controls.Add(checkBox_showdirectory);
            tabPage1.Name = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // button_SplashImage
            // 
            resources.ApplyResources(button_SplashImage, "button_SplashImage");
            button_SplashImage.Name = "button_SplashImage";
            button_SplashImage.UseVisualStyleBackColor = true;
            button_SplashImage.Click += Button_SplashImage_Click;
            // 
            // textBox_SplashImage
            // 
            resources.ApplyResources(textBox_SplashImage, "textBox_SplashImage");
            textBox_SplashImage.Name = "textBox_SplashImage";
            textBox_SplashImage.ReadOnly = true;
            // 
            // checkBox_SplashImage
            // 
            resources.ApplyResources(checkBox_SplashImage, "checkBox_SplashImage");
            checkBox_SplashImage.Name = "checkBox_SplashImage";
            checkBox_SplashImage.UseVisualStyleBackColor = true;
            checkBox_SplashImage.CheckedChanged += CheckBox_SplashImage_CheckedChanged;
            // 
            // checkBox_HideSplash
            // 
            resources.ApplyResources(checkBox_HideSplash, "checkBox_HideSplash");
            checkBox_HideSplash.Name = "checkBox_HideSplash";
            checkBox_HideSplash.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Controls.Add(checkBox_LoopWarning);
            tabPage2.Controls.Add(checkBox_ATWFix);
            tabPage2.Controls.Add(checkBox_ForceWaveConvertion);
            tabPage2.Controls.Add(comboBox_ATWFix);
            tabPage2.Name = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox_LoopWarning
            // 
            resources.ApplyResources(checkBox_LoopWarning, "checkBox_LoopWarning");
            checkBox_LoopWarning.Checked = true;
            checkBox_LoopWarning.CheckState = CheckState.Checked;
            checkBox_LoopWarning.Name = "checkBox_LoopWarning";
            checkBox_LoopWarning.UseVisualStyleBackColor = true;
            checkBox_LoopWarning.CheckedChanged += CheckBox_LoopWarning_CheckedChanged;
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(tabControl_Main);
            panel1.Name = "panel1";
            // 
            // FormPreferences
            // 
            AcceptButton = button_OK;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = button_Cancel;
            ControlBox = false;
            Controls.Add(panel1);
            Controls.Add(button_OK);
            Controls.Add(button_Cancel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FormPreferences";
            Load += FormPreferences_Load;
            tabControl_Main.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button button_Cancel;
        private Button button_OK;
        private CheckBox checkBox_showdirectory;
        private CheckBox checkBox_Updates;
        private CheckBox checkBox_SmoothSamples;
        private CheckBox checkBox_ATWFix;
        private ComboBox comboBox_ATWFix;
        private CheckBox checkBox_ForceWaveConvertion;
        private TabControl tabControl_Main;
        private TabPage tabPage1;
        private CheckBox checkBox_HideSplash;
        private TabPage tabPage2;
        private Panel panel1;
        private Button button_SplashImage;
        private TextBox textBox_SplashImage;
        private CheckBox checkBox_SplashImage;
        private CheckBox checkBox_LoopWarning;
    }
}