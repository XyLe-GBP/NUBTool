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
            menuStrip1 = new MenuStrip();
            fileFToolStripMenuItem = new ToolStripMenuItem();
            openOToolStripMenuItem = new ToolStripMenuItem();
            closeCToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            preferencesSToolStripMenuItem = new ToolStripMenuItem();
            exitXToolStripMenuItem = new ToolStripMenuItem();
            helpHToolStripMenuItem = new ToolStripMenuItem();
            aboutAToolStripMenuItem = new ToolStripMenuItem();
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileFToolStripMenuItem, helpHToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1114, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            fileFToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openOToolStripMenuItem, closeCToolStripMenuItem, toolStripMenuItem1, preferencesSToolStripMenuItem, exitXToolStripMenuItem });
            fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            fileFToolStripMenuItem.Size = new Size(54, 20);
            fileFToolStripMenuItem.Text = "File (&F)";
            // 
            // openOToolStripMenuItem
            // 
            openOToolStripMenuItem.Name = "openOToolStripMenuItem";
            openOToolStripMenuItem.Size = new Size(180, 22);
            openOToolStripMenuItem.Text = "Open (&O)";
            openOToolStripMenuItem.Click += OpenOToolStripMenuItem_Click;
            // 
            // closeCToolStripMenuItem
            // 
            closeCToolStripMenuItem.Enabled = false;
            closeCToolStripMenuItem.Name = "closeCToolStripMenuItem";
            closeCToolStripMenuItem.Size = new Size(180, 22);
            closeCToolStripMenuItem.Text = "Close (&C)";
            closeCToolStripMenuItem.Click += closeCToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(177, 6);
            // 
            // preferencesSToolStripMenuItem
            // 
            preferencesSToolStripMenuItem.Name = "preferencesSToolStripMenuItem";
            preferencesSToolStripMenuItem.Size = new Size(180, 22);
            preferencesSToolStripMenuItem.Text = "Preferences (&S)";
            preferencesSToolStripMenuItem.Click += PreferencesSToolStripMenuItem_Click;
            // 
            // exitXToolStripMenuItem
            // 
            exitXToolStripMenuItem.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            exitXToolStripMenuItem.ForeColor = Color.Red;
            exitXToolStripMenuItem.Name = "exitXToolStripMenuItem";
            exitXToolStripMenuItem.Size = new Size(180, 22);
            exitXToolStripMenuItem.Text = "Exit (&X)";
            exitXToolStripMenuItem.Click += ExitXToolStripMenuItem_Click;
            // 
            // helpHToolStripMenuItem
            // 
            helpHToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            helpHToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutAToolStripMenuItem });
            helpHToolStripMenuItem.Name = "helpHToolStripMenuItem";
            helpHToolStripMenuItem.Size = new Size(64, 20);
            helpHToolStripMenuItem.Text = "Help (&H)";
            // 
            // aboutAToolStripMenuItem
            // 
            aboutAToolStripMenuItem.Name = "aboutAToolStripMenuItem";
            aboutAToolStripMenuItem.Size = new Size(180, 22);
            aboutAToolStripMenuItem.Text = "About (&A)";
            aboutAToolStripMenuItem.Click += AboutAToolStripMenuItem_Click;
            // 
            // label_LS
            // 
            label_LS.AutoSize = true;
            label_LS.Enabled = false;
            label_LS.Location = new Point(10, 400);
            label_LS.Name = "label_LS";
            label_LS.Size = new Size(107, 15);
            label_LS.TabIndex = 1;
            label_LS.Text = "LoopStart Samples:";
            // 
            // label_LE
            // 
            label_LE.AutoSize = true;
            label_LE.Enabled = false;
            label_LE.Location = new Point(14, 425);
            label_LE.Name = "label_LE";
            label_LE.Size = new Size(103, 15);
            label_LE.TabIndex = 2;
            label_LE.Text = "LoopEnd Samples:";
            // 
            // checkBox_LoopEnable
            // 
            checkBox_LoopEnable.AutoSize = true;
            checkBox_LoopEnable.Enabled = false;
            checkBox_LoopEnable.Location = new Point(221, 349);
            checkBox_LoopEnable.Name = "checkBox_LoopEnable";
            checkBox_LoopEnable.Size = new Size(91, 19);
            checkBox_LoopEnable.TabIndex = 3;
            checkBox_LoopEnable.Text = "Enable Loop";
            checkBox_LoopEnable.UseVisualStyleBackColor = true;
            checkBox_LoopEnable.CheckedChanged += CheckBox_LoopEnable_CheckedChanged;
            // 
            // textBox_LoopStart
            // 
            textBox_LoopStart.Enabled = false;
            textBox_LoopStart.ImeMode = ImeMode.Disable;
            textBox_LoopStart.Location = new Point(119, 397);
            textBox_LoopStart.MaxLength = 11;
            textBox_LoopStart.Name = "textBox_LoopStart";
            textBox_LoopStart.ShortcutsEnabled = false;
            textBox_LoopStart.Size = new Size(193, 23);
            textBox_LoopStart.TabIndex = 4;
            textBox_LoopStart.TextChanged += TextBox_LoopStart_TextChanged;
            textBox_LoopStart.KeyPress += TextBox_LoopStart_KeyPress;
            // 
            // textBox_LoopEnd
            // 
            textBox_LoopEnd.Enabled = false;
            textBox_LoopEnd.ImeMode = ImeMode.Disable;
            textBox_LoopEnd.Location = new Point(119, 422);
            textBox_LoopEnd.MaxLength = 11;
            textBox_LoopEnd.Name = "textBox_LoopEnd";
            textBox_LoopEnd.ShortcutsEnabled = false;
            textBox_LoopEnd.Size = new Size(193, 23);
            textBox_LoopEnd.TabIndex = 5;
            textBox_LoopEnd.TextChanged += TextBox_LoopEnd_TextChanged;
            textBox_LoopEnd.KeyPress += TextBox_LoopEnd_KeyPress;
            // 
            // label_Channel
            // 
            label_Channel.AutoSize = true;
            label_Channel.Enabled = false;
            label_Channel.Location = new Point(17, 323);
            label_Channel.Name = "label_Channel";
            label_Channel.Size = new Size(58, 15);
            label_Channel.TabIndex = 6;
            label_Channel.Text = "Channels:";
            // 
            // comboBox_Channels
            // 
            comboBox_Channels.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Channels.Enabled = false;
            comboBox_Channels.FormattingEnabled = true;
            comboBox_Channels.Items.AddRange(new object[] { "1 (mono)", "2 (stereo)", "3", "4", "5", "6" });
            comboBox_Channels.Location = new Point(81, 320);
            comboBox_Channels.Name = "comboBox_Channels";
            comboBox_Channels.Size = new Size(167, 23);
            comboBox_Channels.TabIndex = 7;
            comboBox_Channels.SelectedIndexChanged += ComboBox_Channels_SelectedIndexChanged;
            // 
            // button_Run
            // 
            button_Run.Dock = DockStyle.Bottom;
            button_Run.Enabled = false;
            button_Run.Location = new Point(0, 451);
            button_Run.Name = "button_Run";
            button_Run.Size = new Size(315, 45);
            button_Run.TabIndex = 8;
            button_Run.Text = "Convert";
            button_Run.UseVisualStyleBackColor = true;
            button_Run.Click += Button_Run_Click;
            // 
            // comboBox_Volume
            // 
            comboBox_Volume.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Volume.Enabled = false;
            comboBox_Volume.FormattingEnabled = true;
            comboBox_Volume.Items.AddRange(new object[] { "Low (0x3F00)", "Normal (0x4080)", "High (0x40C0)", "Ultra (0x4100)", "Custom" });
            comboBox_Volume.Location = new Point(81, 346);
            comboBox_Volume.Name = "comboBox_Volume";
            comboBox_Volume.Size = new Size(137, 23);
            comboBox_Volume.TabIndex = 9;
            comboBox_Volume.SelectedIndexChanged += ComboBox_Volume_SelectedIndexChanged;
            // 
            // label_Volume
            // 
            label_Volume.AutoSize = true;
            label_Volume.Enabled = false;
            label_Volume.Location = new Point(26, 349);
            label_Volume.Name = "label_Volume";
            label_Volume.Size = new Size(49, 15);
            label_Volume.TabIndex = 10;
            label_Volume.Text = "Volume:";
            // 
            // label_Samplerate
            // 
            label_Samplerate.AutoSize = true;
            label_Samplerate.Enabled = false;
            label_Samplerate.Location = new Point(7, 297);
            label_Samplerate.Name = "label_Samplerate";
            label_Samplerate.Size = new Size(68, 15);
            label_Samplerate.TabIndex = 11;
            label_Samplerate.Text = "Samplerate:";
            // 
            // comboBox_Samplerate
            // 
            comboBox_Samplerate.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Samplerate.Enabled = false;
            comboBox_Samplerate.FormattingEnabled = true;
            comboBox_Samplerate.Items.AddRange(new object[] { "44100 Hz (Default)", "48000 Hz" });
            comboBox_Samplerate.Location = new Point(81, 294);
            comboBox_Samplerate.Name = "comboBox_Samplerate";
            comboBox_Samplerate.Size = new Size(120, 23);
            comboBox_Samplerate.TabIndex = 12;
            comboBox_Samplerate.SelectedIndexChanged += ComboBox_Samplerate_SelectedIndexChanged;
            // 
            // panel_Control
            // 
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
            panel_Control.Dock = DockStyle.Right;
            panel_Control.Location = new Point(799, 24);
            panel_Control.Name = "panel_Control";
            panel_Control.Size = new Size(315, 496);
            panel_Control.TabIndex = 11;
            // 
            // label_nos_hex
            // 
            label_nos_hex.AutoSize = true;
            label_nos_hex.Enabled = false;
            label_nos_hex.Location = new Point(281, 323);
            label_nos_hex.Name = "label_nos_hex";
            label_nos_hex.Size = new Size(25, 15);
            label_nos_hex.TabIndex = 36;
            label_nos_hex.Text = "0x1";
            // 
            // comboBox_version
            // 
            comboBox_version.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_version.Enabled = false;
            comboBox_version.FormattingEnabled = true;
            comboBox_version.Items.AddRange(new object[] { "v2.1 [0x01020100] (Normal)", "v2.1 [0x00020100] (Common)", "v2.0 [0x00020000] (X360, etc)" });
            comboBox_version.Location = new Point(99, 242);
            comboBox_version.Name = "comboBox_version";
            comboBox_version.Size = new Size(213, 23);
            comboBox_version.TabIndex = 19;
            comboBox_version.SelectedIndexChanged += ComboBox_version_SelectedIndexChanged;
            // 
            // label_version
            // 
            label_version.AutoSize = true;
            label_version.Enabled = false;
            label_version.Location = new Point(1, 245);
            label_version.Name = "label_version";
            label_version.Size = new Size(99, 15);
            label_version.TabIndex = 18;
            label_version.Text = "nuSound Version:";
            // 
            // label_id_hex
            // 
            label_id_hex.AutoSize = true;
            label_id_hex.Enabled = false;
            label_id_hex.Location = new Point(281, 271);
            label_id_hex.Name = "label_id_hex";
            label_id_hex.Size = new Size(25, 15);
            label_id_hex.TabIndex = 35;
            label_id_hex.Text = "0x0";
            // 
            // textBox_nos
            // 
            textBox_nos.Enabled = false;
            textBox_nos.ImeMode = ImeMode.Disable;
            textBox_nos.Location = new Point(249, 320);
            textBox_nos.MaxLength = 2;
            textBox_nos.Name = "textBox_nos";
            textBox_nos.ShortcutsEnabled = false;
            textBox_nos.Size = new Size(30, 23);
            textBox_nos.TabIndex = 34;
            textBox_nos.TextChanged += TextBox_nos_TextChanged;
            textBox_nos.KeyPress += TextBox_nos_KeyPress;
            // 
            // label_nos
            // 
            label_nos.AutoSize = true;
            label_nos.Enabled = false;
            label_nos.Location = new Point(204, 297);
            label_nos.Name = "label_nos";
            label_nos.Size = new Size(111, 15);
            label_nos.TabIndex = 33;
            label_nos.Text = "Number of Streams:";
            // 
            // textBox_id
            // 
            textBox_id.Enabled = false;
            textBox_id.ImeMode = ImeMode.Disable;
            textBox_id.Location = new Point(249, 268);
            textBox_id.MaxLength = 3;
            textBox_id.Name = "textBox_id";
            textBox_id.ShortcutsEnabled = false;
            textBox_id.Size = new Size(30, 23);
            textBox_id.TabIndex = 32;
            textBox_id.TextChanged += TextBox_id_TextChanged;
            textBox_id.KeyPress += TextBox_id_KeyPress;
            // 
            // label_id_c
            // 
            label_id_c.AutoSize = true;
            label_id_c.Enabled = false;
            label_id_c.Location = new Point(228, 271);
            label_id_c.Name = "label_id_c";
            label_id_c.Size = new Size(21, 15);
            label_id_c.TabIndex = 31;
            label_id_c.Text = "ID:";
            // 
            // label_NStream
            // 
            label_NStream.AutoSize = true;
            label_NStream.Location = new Point(130, 122);
            label_NStream.Name = "label_NStream";
            label_NStream.Size = new Size(133, 15);
            label_NStream.TabIndex = 30;
            label_NStream.Text = "NUMBER_OF_STREAMS:";
            label_NStream.Visible = false;
            // 
            // label_id
            // 
            label_id.AutoSize = true;
            label_id.Location = new Point(3, 122);
            label_id.Name = "label_id";
            label_id.Size = new Size(70, 15);
            label_id.TabIndex = 29;
            label_id.Text = "STREAM_ID:";
            label_id.Visible = false;
            // 
            // label_Filename
            // 
            label_Filename.Location = new Point(3, 0);
            label_Filename.Name = "label_Filename";
            label_Filename.Size = new Size(309, 30);
            label_Filename.TabIndex = 28;
            label_Filename.Text = "FILE_NAME:";
            label_Filename.Visible = false;
            // 
            // label_NoInfo
            // 
            label_NoInfo.AutoSize = true;
            label_NoInfo.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            label_NoInfo.Location = new Point(48, 94);
            label_NoInfo.Name = "label_NoInfo";
            label_NoInfo.Size = new Size(216, 37);
            label_NoInfo.TabIndex = 22;
            label_NoInfo.Text = "No Informations.";
            // 
            // label_Samplerate_Info
            // 
            label_Samplerate_Info.AutoSize = true;
            label_Samplerate_Info.Location = new Point(3, 217);
            label_Samplerate_Info.Name = "label_Samplerate_Info";
            label_Samplerate_Info.Size = new Size(85, 15);
            label_Samplerate_Info.TabIndex = 27;
            label_Samplerate_Info.Text = "SAMPLE_LATE:";
            label_Samplerate_Info.Visible = false;
            // 
            // label_Channel_Info
            // 
            label_Channel_Info.AutoSize = true;
            label_Channel_Info.Location = new Point(194, 198);
            label_Channel_Info.Name = "label_Channel_Info";
            label_Channel_Info.Size = new Size(70, 15);
            label_Channel_Info.TabIndex = 26;
            label_Channel_Info.Text = "CHANNELS:";
            label_Channel_Info.Visible = false;
            // 
            // label_Volume_Info
            // 
            label_Volume_Info.AutoSize = true;
            label_Volume_Info.Location = new Point(3, 198);
            label_Volume_Info.Name = "label_Volume_Info";
            label_Volume_Info.Size = new Size(57, 15);
            label_Volume_Info.TabIndex = 25;
            label_Volume_Info.Text = "VOLUME:";
            label_Volume_Info.Visible = false;
            // 
            // label_lePosition
            // 
            label_lePosition.AutoSize = true;
            label_lePosition.Location = new Point(3, 179);
            label_lePosition.Name = "label_lePosition";
            label_lePosition.Size = new Size(69, 15);
            label_lePosition.TabIndex = 24;
            label_lePosition.Text = "LOOP_END:";
            label_lePosition.Visible = false;
            // 
            // label_lsPosition
            // 
            label_lsPosition.AutoSize = true;
            label_lsPosition.Location = new Point(3, 159);
            label_lsPosition.Name = "label_lsPosition";
            label_lsPosition.Size = new Size(78, 15);
            label_lsPosition.TabIndex = 23;
            label_lsPosition.Text = "LOOP_START:";
            label_lsPosition.Visible = false;
            // 
            // label_Totalstream
            // 
            label_Totalstream.AutoSize = true;
            label_Totalstream.Location = new Point(3, 140);
            label_Totalstream.Name = "label_Totalstream";
            label_Totalstream.Size = new Size(94, 15);
            label_Totalstream.TabIndex = 21;
            label_Totalstream.Text = "TOTAL_STREAM:";
            label_Totalstream.Visible = false;
            // 
            // label_OrigFilepath
            // 
            label_OrigFilepath.AutoEllipsis = true;
            label_OrigFilepath.Location = new Point(3, 42);
            label_OrigFilepath.Name = "label_OrigFilepath";
            label_OrigFilepath.Size = new Size(309, 80);
            label_OrigFilepath.TabIndex = 20;
            label_OrigFilepath.Text = "FILE_PATH:";
            label_OrigFilepath.Visible = false;
            // 
            // label_CustomVolHex
            // 
            label_CustomVolHex.AutoSize = true;
            label_CustomVolHex.Enabled = false;
            label_CustomVolHex.Location = new Point(165, 375);
            label_CustomVolHex.Name = "label_CustomVolHex";
            label_CustomVolHex.Size = new Size(52, 15);
            label_CustomVolHex.TabIndex = 17;
            label_CustomVolHex.Text = "Hex: 0x0";
            // 
            // label_CustomVol
            // 
            label_CustomVol.AutoSize = true;
            label_CustomVol.Enabled = false;
            label_CustomVol.Location = new Point(38, 375);
            label_CustomVol.Name = "label_CustomVol";
            label_CustomVol.Size = new Size(79, 15);
            label_CustomVol.TabIndex = 16;
            label_CustomVol.Text = "Volume (dec):";
            // 
            // textBox_CustomVol
            // 
            textBox_CustomVol.Enabled = false;
            textBox_CustomVol.ImeMode = ImeMode.Disable;
            textBox_CustomVol.Location = new Point(119, 372);
            textBox_CustomVol.MaxLength = 5;
            textBox_CustomVol.Name = "textBox_CustomVol";
            textBox_CustomVol.ShortcutsEnabled = false;
            textBox_CustomVol.Size = new Size(44, 23);
            textBox_CustomVol.TabIndex = 15;
            textBox_CustomVol.TextChanged += TextBox_CustomVol_TextChanged;
            textBox_CustomVol.KeyPress += TextBox_CustomVol_KeyPress;
            // 
            // label_Format
            // 
            label_Format.AutoSize = true;
            label_Format.Enabled = false;
            label_Format.Location = new Point(1, 271);
            label_Format.Name = "label_Format";
            label_Format.Size = new Size(74, 15);
            label_Format.TabIndex = 14;
            label_Format.Text = "NUB Format:";
            // 
            // comboBox_Format
            // 
            comboBox_Format.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Format.Enabled = false;
            comboBox_Format.FormattingEnabled = true;
            comboBox_Format.Items.AddRange(new object[] { "Signed Big Endian", "Signed Little Endian", "Unsigned Big Endian", "Unsigned Little Endian" });
            comboBox_Format.Location = new Point(81, 268);
            comboBox_Format.Name = "comboBox_Format";
            comboBox_Format.Size = new Size(145, 23);
            comboBox_Format.TabIndex = 13;
            comboBox_Format.SelectedIndexChanged += ComboBox_Format_SelectedIndexChanged;
            // 
            // panel_Main
            // 
            panel_Main.Controls.Add(label_Openhere);
            panel_Main.Dock = DockStyle.Fill;
            panel_Main.Location = new Point(0, 24);
            panel_Main.Name = "panel_Main";
            panel_Main.Size = new Size(799, 496);
            panel_Main.TabIndex = 12;
            // 
            // label_Openhere
            // 
            label_Openhere.AllowDrop = true;
            label_Openhere.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label_Openhere.AutoSize = true;
            label_Openhere.Font = new Font("Yu Gothic UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            label_Openhere.Location = new Point(107, 193);
            label_Openhere.Name = "label_Openhere";
            label_Openhere.Size = new Size(637, 90);
            label_Openhere.TabIndex = 0;
            label_Openhere.Text = "Drag and drop the corresponding file here\r\nor open the file...";
            label_Openhere.TextAlign = ContentAlignment.MiddleCenter;
            label_Openhere.DragDrop += Label_Openhere_DragDrop;
            label_Openhere.DragEnter += Label_Openhere_DragEnter;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1114, 520);
            Controls.Add(panel_Main);
            Controls.Add(panel_Control);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NUBTool";
            FormClosed += FormMain_FormClosed;
            Load += FormMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel_Control.ResumeLayout(false);
            panel_Control.PerformLayout();
            panel_Main.ResumeLayout(false);
            panel_Main.PerformLayout();
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
        private CheckBox checkBox_LoopEnable;
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
    }
}