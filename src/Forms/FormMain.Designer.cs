namespace NUBTool
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            menuStrip1 = new MenuStrip();
            fileFToolStripMenuItem = new ToolStripMenuItem();
            openOToolStripMenuItem = new ToolStripMenuItem();
            closeCToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            preferencesSToolStripMenuItem = new ToolStripMenuItem();
            exitXToolStripMenuItem = new ToolStripMenuItem();
            helpHToolStripMenuItem = new ToolStripMenuItem();
            aboutAToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            checkForUpdatesUToolStripMenuItem = new ToolStripMenuItem();
            label_LS = new Label();
            label_LE = new Label();
            checkBox_LoopEnable = new CheckBox();
            textBox_LoopStart = new TextBox();
            textBox_LoopEnd = new TextBox();
            label_Channel = new Label();
            comboBox_Channels = new ComboBox();
            button_Run = new Button();
            comboBox_Volume = new ComboBox();
            label_Volume = new Label();
            label_Samplerate = new Label();
            comboBox_Samplerate = new ComboBox();
            panel_Control = new Panel();
            label_nos_hex = new Label();
            comboBox_version = new ComboBox();
            label_version = new Label();
            label_id_hex = new Label();
            textBox_nos = new TextBox();
            label_nos = new Label();
            textBox_id = new TextBox();
            label_id_c = new Label();
            label_NStream = new Label();
            label_id = new Label();
            label_Filename = new Label();
            label_NoInfo = new Label();
            label_Samplerate_Info = new Label();
            label_Channel_Info = new Label();
            label_Volume_Info = new Label();
            label_lePosition = new Label();
            label_lsPosition = new Label();
            label_Totalstream = new Label();
            label_OrigFilepath = new Label();
            label_CustomVolHex = new Label();
            label_CustomVol = new Label();
            textBox_CustomVol = new TextBox();
            label_Format = new Label();
            comboBox_Format = new ComboBox();
            panel_Main = new Panel();
            label_Openhere = new Label();
            menuStrip1.SuspendLayout();
            panel_Control.SuspendLayout();
            panel_Main.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            resources.ApplyResources(menuStrip1, "menuStrip1");
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileFToolStripMenuItem, helpHToolStripMenuItem });
            menuStrip1.Name = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            resources.ApplyResources(fileFToolStripMenuItem, "fileFToolStripMenuItem");
            fileFToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openOToolStripMenuItem, closeCToolStripMenuItem, toolStripMenuItem1, preferencesSToolStripMenuItem, exitXToolStripMenuItem });
            fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            // 
            // openOToolStripMenuItem
            // 
            resources.ApplyResources(openOToolStripMenuItem, "openOToolStripMenuItem");
            openOToolStripMenuItem.Name = "openOToolStripMenuItem";
            openOToolStripMenuItem.Click += OpenOToolStripMenuItem_Click;
            // 
            // closeCToolStripMenuItem
            // 
            resources.ApplyResources(closeCToolStripMenuItem, "closeCToolStripMenuItem");
            closeCToolStripMenuItem.Name = "closeCToolStripMenuItem";
            closeCToolStripMenuItem.Click += CloseCToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(toolStripMenuItem1, "toolStripMenuItem1");
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // preferencesSToolStripMenuItem
            // 
            resources.ApplyResources(preferencesSToolStripMenuItem, "preferencesSToolStripMenuItem");
            preferencesSToolStripMenuItem.Name = "preferencesSToolStripMenuItem";
            preferencesSToolStripMenuItem.Click += PreferencesSToolStripMenuItem_Click;
            // 
            // exitXToolStripMenuItem
            // 
            resources.ApplyResources(exitXToolStripMenuItem, "exitXToolStripMenuItem");
            exitXToolStripMenuItem.ForeColor = Color.Red;
            exitXToolStripMenuItem.Name = "exitXToolStripMenuItem";
            exitXToolStripMenuItem.Click += ExitXToolStripMenuItem_Click;
            // 
            // helpHToolStripMenuItem
            // 
            resources.ApplyResources(helpHToolStripMenuItem, "helpHToolStripMenuItem");
            helpHToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            helpHToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutAToolStripMenuItem, toolStripMenuItem2, checkForUpdatesUToolStripMenuItem });
            helpHToolStripMenuItem.Name = "helpHToolStripMenuItem";
            // 
            // aboutAToolStripMenuItem
            // 
            resources.ApplyResources(aboutAToolStripMenuItem, "aboutAToolStripMenuItem");
            aboutAToolStripMenuItem.Name = "aboutAToolStripMenuItem";
            aboutAToolStripMenuItem.Click += AboutAToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(toolStripMenuItem2, "toolStripMenuItem2");
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // checkForUpdatesUToolStripMenuItem
            // 
            resources.ApplyResources(checkForUpdatesUToolStripMenuItem, "checkForUpdatesUToolStripMenuItem");
            checkForUpdatesUToolStripMenuItem.Name = "checkForUpdatesUToolStripMenuItem";
            checkForUpdatesUToolStripMenuItem.Click += CheckForUpdatesUToolStripMenuItem_Click;
            // 
            // label_LS
            // 
            resources.ApplyResources(label_LS, "label_LS");
            label_LS.Name = "label_LS";
            // 
            // label_LE
            // 
            resources.ApplyResources(label_LE, "label_LE");
            label_LE.Name = "label_LE";
            // 
            // checkBox_LoopEnable
            // 
            resources.ApplyResources(checkBox_LoopEnable, "checkBox_LoopEnable");
            checkBox_LoopEnable.Name = "checkBox_LoopEnable";
            checkBox_LoopEnable.UseVisualStyleBackColor = true;
            checkBox_LoopEnable.CheckedChanged += CheckBox_LoopEnable_CheckedChanged;
            // 
            // textBox_LoopStart
            // 
            resources.ApplyResources(textBox_LoopStart, "textBox_LoopStart");
            textBox_LoopStart.Name = "textBox_LoopStart";
            textBox_LoopStart.ShortcutsEnabled = false;
            textBox_LoopStart.TextChanged += TextBox_LoopStart_TextChanged;
            textBox_LoopStart.KeyPress += TextBox_LoopStart_KeyPress;
            // 
            // textBox_LoopEnd
            // 
            resources.ApplyResources(textBox_LoopEnd, "textBox_LoopEnd");
            textBox_LoopEnd.Name = "textBox_LoopEnd";
            textBox_LoopEnd.ShortcutsEnabled = false;
            textBox_LoopEnd.TextChanged += TextBox_LoopEnd_TextChanged;
            textBox_LoopEnd.KeyPress += TextBox_LoopEnd_KeyPress;
            // 
            // label_Channel
            // 
            resources.ApplyResources(label_Channel, "label_Channel");
            label_Channel.Name = "label_Channel";
            // 
            // comboBox_Channels
            // 
            resources.ApplyResources(comboBox_Channels, "comboBox_Channels");
            comboBox_Channels.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Channels.FormattingEnabled = true;
            comboBox_Channels.Items.AddRange(new object[] { resources.GetString("comboBox_Channels.Items"), resources.GetString("comboBox_Channels.Items1"), resources.GetString("comboBox_Channels.Items2"), resources.GetString("comboBox_Channels.Items3"), resources.GetString("comboBox_Channels.Items4"), resources.GetString("comboBox_Channels.Items5") });
            comboBox_Channels.Name = "comboBox_Channels";
            comboBox_Channels.SelectedIndexChanged += ComboBox_Channels_SelectedIndexChanged;
            // 
            // button_Run
            // 
            resources.ApplyResources(button_Run, "button_Run");
            button_Run.Name = "button_Run";
            button_Run.UseVisualStyleBackColor = true;
            button_Run.Click += Button_Run_Click;
            // 
            // comboBox_Volume
            // 
            resources.ApplyResources(comboBox_Volume, "comboBox_Volume");
            comboBox_Volume.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Volume.FormattingEnabled = true;
            comboBox_Volume.Items.AddRange(new object[] { resources.GetString("comboBox_Volume.Items"), resources.GetString("comboBox_Volume.Items1"), resources.GetString("comboBox_Volume.Items2"), resources.GetString("comboBox_Volume.Items3"), resources.GetString("comboBox_Volume.Items4") });
            comboBox_Volume.Name = "comboBox_Volume";
            comboBox_Volume.SelectedIndexChanged += ComboBox_Volume_SelectedIndexChanged;
            // 
            // label_Volume
            // 
            resources.ApplyResources(label_Volume, "label_Volume");
            label_Volume.Name = "label_Volume";
            // 
            // label_Samplerate
            // 
            resources.ApplyResources(label_Samplerate, "label_Samplerate");
            label_Samplerate.Name = "label_Samplerate";
            // 
            // comboBox_Samplerate
            // 
            resources.ApplyResources(comboBox_Samplerate, "comboBox_Samplerate");
            comboBox_Samplerate.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Samplerate.FormattingEnabled = true;
            comboBox_Samplerate.Items.AddRange(new object[] { resources.GetString("comboBox_Samplerate.Items"), resources.GetString("comboBox_Samplerate.Items1") });
            comboBox_Samplerate.Name = "comboBox_Samplerate";
            comboBox_Samplerate.SelectedIndexChanged += ComboBox_Samplerate_SelectedIndexChanged;
            // 
            // panel_Control
            // 
            resources.ApplyResources(panel_Control, "panel_Control");
            panel_Control.Controls.Add(label_nos_hex);
            panel_Control.Controls.Add(comboBox_version);
            panel_Control.Controls.Add(label_version);
            panel_Control.Controls.Add(label_id_hex);
            panel_Control.Controls.Add(textBox_nos);
            panel_Control.Controls.Add(label_nos);
            panel_Control.Controls.Add(textBox_id);
            panel_Control.Controls.Add(label_id_c);
            panel_Control.Controls.Add(label_NStream);
            panel_Control.Controls.Add(label_id);
            panel_Control.Controls.Add(label_Filename);
            panel_Control.Controls.Add(label_NoInfo);
            panel_Control.Controls.Add(label_Samplerate_Info);
            panel_Control.Controls.Add(label_Channel_Info);
            panel_Control.Controls.Add(label_Volume_Info);
            panel_Control.Controls.Add(label_lePosition);
            panel_Control.Controls.Add(label_lsPosition);
            panel_Control.Controls.Add(label_Totalstream);
            panel_Control.Controls.Add(label_OrigFilepath);
            panel_Control.Controls.Add(label_CustomVolHex);
            panel_Control.Controls.Add(label_CustomVol);
            panel_Control.Controls.Add(textBox_CustomVol);
            panel_Control.Controls.Add(textBox_LoopEnd);
            panel_Control.Controls.Add(label_Format);
            panel_Control.Controls.Add(textBox_LoopStart);
            panel_Control.Controls.Add(label_LE);
            panel_Control.Controls.Add(comboBox_Volume);
            panel_Control.Controls.Add(label_LS);
            panel_Control.Controls.Add(comboBox_Samplerate);
            panel_Control.Controls.Add(label_Volume);
            panel_Control.Controls.Add(checkBox_LoopEnable);
            panel_Control.Controls.Add(comboBox_Format);
            panel_Control.Controls.Add(button_Run);
            panel_Control.Controls.Add(label_Samplerate);
            panel_Control.Controls.Add(comboBox_Channels);
            panel_Control.Controls.Add(label_Channel);
            panel_Control.Name = "panel_Control";
            // 
            // label_nos_hex
            // 
            resources.ApplyResources(label_nos_hex, "label_nos_hex");
            label_nos_hex.Name = "label_nos_hex";
            // 
            // comboBox_version
            // 
            resources.ApplyResources(comboBox_version, "comboBox_version");
            comboBox_version.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_version.FormattingEnabled = true;
            comboBox_version.Items.AddRange(new object[] { resources.GetString("comboBox_version.Items"), resources.GetString("comboBox_version.Items1"), resources.GetString("comboBox_version.Items2") });
            comboBox_version.Name = "comboBox_version";
            comboBox_version.SelectedIndexChanged += ComboBox_version_SelectedIndexChanged;
            // 
            // label_version
            // 
            resources.ApplyResources(label_version, "label_version");
            label_version.Name = "label_version";
            // 
            // label_id_hex
            // 
            resources.ApplyResources(label_id_hex, "label_id_hex");
            label_id_hex.Name = "label_id_hex";
            // 
            // textBox_nos
            // 
            resources.ApplyResources(textBox_nos, "textBox_nos");
            textBox_nos.Name = "textBox_nos";
            textBox_nos.ShortcutsEnabled = false;
            textBox_nos.TextChanged += TextBox_nos_TextChanged;
            textBox_nos.KeyPress += TextBox_nos_KeyPress;
            // 
            // label_nos
            // 
            resources.ApplyResources(label_nos, "label_nos");
            label_nos.Name = "label_nos";
            // 
            // textBox_id
            // 
            resources.ApplyResources(textBox_id, "textBox_id");
            textBox_id.Name = "textBox_id";
            textBox_id.ShortcutsEnabled = false;
            textBox_id.TextChanged += TextBox_id_TextChanged;
            textBox_id.KeyPress += TextBox_id_KeyPress;
            // 
            // label_id_c
            // 
            resources.ApplyResources(label_id_c, "label_id_c");
            label_id_c.Name = "label_id_c";
            // 
            // label_NStream
            // 
            resources.ApplyResources(label_NStream, "label_NStream");
            label_NStream.Name = "label_NStream";
            // 
            // label_id
            // 
            resources.ApplyResources(label_id, "label_id");
            label_id.Name = "label_id";
            // 
            // label_Filename
            // 
            resources.ApplyResources(label_Filename, "label_Filename");
            label_Filename.Name = "label_Filename";
            // 
            // label_NoInfo
            // 
            resources.ApplyResources(label_NoInfo, "label_NoInfo");
            label_NoInfo.Name = "label_NoInfo";
            // 
            // label_Samplerate_Info
            // 
            resources.ApplyResources(label_Samplerate_Info, "label_Samplerate_Info");
            label_Samplerate_Info.Name = "label_Samplerate_Info";
            // 
            // label_Channel_Info
            // 
            resources.ApplyResources(label_Channel_Info, "label_Channel_Info");
            label_Channel_Info.Name = "label_Channel_Info";
            // 
            // label_Volume_Info
            // 
            resources.ApplyResources(label_Volume_Info, "label_Volume_Info");
            label_Volume_Info.Name = "label_Volume_Info";
            // 
            // label_lePosition
            // 
            resources.ApplyResources(label_lePosition, "label_lePosition");
            label_lePosition.Name = "label_lePosition";
            // 
            // label_lsPosition
            // 
            resources.ApplyResources(label_lsPosition, "label_lsPosition");
            label_lsPosition.Name = "label_lsPosition";
            // 
            // label_Totalstream
            // 
            resources.ApplyResources(label_Totalstream, "label_Totalstream");
            label_Totalstream.Name = "label_Totalstream";
            // 
            // label_OrigFilepath
            // 
            resources.ApplyResources(label_OrigFilepath, "label_OrigFilepath");
            label_OrigFilepath.AutoEllipsis = true;
            label_OrigFilepath.Name = "label_OrigFilepath";
            // 
            // label_CustomVolHex
            // 
            resources.ApplyResources(label_CustomVolHex, "label_CustomVolHex");
            label_CustomVolHex.Name = "label_CustomVolHex";
            // 
            // label_CustomVol
            // 
            resources.ApplyResources(label_CustomVol, "label_CustomVol");
            label_CustomVol.Name = "label_CustomVol";
            // 
            // textBox_CustomVol
            // 
            resources.ApplyResources(textBox_CustomVol, "textBox_CustomVol");
            textBox_CustomVol.Name = "textBox_CustomVol";
            textBox_CustomVol.ShortcutsEnabled = false;
            textBox_CustomVol.TextChanged += TextBox_CustomVol_TextChanged;
            textBox_CustomVol.KeyPress += TextBox_CustomVol_KeyPress;
            // 
            // label_Format
            // 
            resources.ApplyResources(label_Format, "label_Format");
            label_Format.Name = "label_Format";
            // 
            // comboBox_Format
            // 
            resources.ApplyResources(comboBox_Format, "comboBox_Format");
            comboBox_Format.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Format.FormattingEnabled = true;
            comboBox_Format.Items.AddRange(new object[] { resources.GetString("comboBox_Format.Items"), resources.GetString("comboBox_Format.Items1"), resources.GetString("comboBox_Format.Items2"), resources.GetString("comboBox_Format.Items3") });
            comboBox_Format.Name = "comboBox_Format";
            comboBox_Format.SelectedIndexChanged += ComboBox_Format_SelectedIndexChanged;
            // 
            // panel_Main
            // 
            resources.ApplyResources(panel_Main, "panel_Main");
            panel_Main.Controls.Add(label_Openhere);
            panel_Main.Name = "panel_Main";
            // 
            // label_Openhere
            // 
            resources.ApplyResources(label_Openhere, "label_Openhere");
            label_Openhere.AllowDrop = true;
            label_Openhere.Name = "label_Openhere";
            label_Openhere.DragDrop += Label_Openhere_DragDrop;
            label_Openhere.DragEnter += Label_Openhere_DragEnter;
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel_Main);
            Controls.Add(panel_Control);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "FormMain";
            FormClosed += FormMain_FormClosed;
            Load += FormMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel_Control.ResumeLayout(false);
            panel_Control.PerformLayout();
            panel_Main.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileFToolStripMenuItem;
        private ToolStripMenuItem openOToolStripMenuItem;
        private ToolStripMenuItem closeCToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitXToolStripMenuItem;
        private ToolStripMenuItem helpHToolStripMenuItem;
        private ToolStripMenuItem aboutAToolStripMenuItem;
        private Label label_LS;
        private Label label_LE;
        private Label label_Channel;
        private ComboBox comboBox_Channels;
        private Button button_Run;
        private ComboBox comboBox_Volume;
        private Label label_Volume;
        private ComboBox comboBox_Samplerate;
        private Label label_Samplerate;
        private Panel panel_Control;
        private Panel panel_Main;
        private Label label_Format;
        private ComboBox comboBox_Format;
        private Label label_Openhere;
        private ToolStripMenuItem preferencesSToolStripMenuItem;
        internal TextBox textBox_LoopStart;
        internal TextBox textBox_LoopEnd;
        private Label label_CustomVol;
        private TextBox textBox_CustomVol;
        private Label label_CustomVolHex;
        private ComboBox comboBox_version;
        private Label label_version;
        private Label label_OrigFilepath;
        private Label label_Totalstream;
        private Label label_NoInfo;
        private Label label_lsPosition;
        private Label label_lePosition;
        private Label label_Samplerate_Info;
        private Label label_Channel_Info;
        private Label label_Volume_Info;
        private Label label_Filename;
        private Label label_NStream;
        private Label label_id;
        private Label label_id_c;
        private TextBox textBox_nos;
        private Label label_nos;
        private TextBox textBox_id;
        private Label label_nos_hex;
        private Label label_id_hex;
        internal CheckBox checkBox_LoopEnable;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem checkForUpdatesUToolStripMenuItem;
    }
}