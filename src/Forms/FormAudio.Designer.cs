namespace NUBTool
{
    partial class FormAudio
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAudio));
            volumeSlider1 = new NAudio.Gui.VolumeSlider();
            button_Play = new Button();
            button_Stop = new Button();
            button_Next = new Button();
            button_Prev = new Button();
            timer_Reload = new System.Windows.Forms.Timer(components);
            label_Length = new Label();
            label_Plength = new Label();
            label_Samples = new Label();
            label_Psamples = new Label();
            label_File = new Label();
            numericUpDown_LoopStart = new NumericUpDown();
            numericUpDown_LoopEnd = new NumericUpDown();
            button_LS_Current = new Button();
            button_LE_Current = new Button();
            button_SetStart = new Button();
            button_SetEnd = new Button();
            label_LoopStart = new Label();
            label_LoopEnd = new Label();
            customTrackBar_LoopEnd = new src.Controls.CustomTrackBar();
            customTrackBar_Main = new src.Controls.CustomTrackBar();
            customTrackBar_LoopStart = new src.Controls.CustomTrackBar();
            label_main = new Label();
            label_lstart = new Label();
            label_lend = new Label();
            label_StartTweak = new Label();
            label_EndTweak = new Label();
            label_Volume = new Label();
            label_FileSize = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_LoopStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_LoopEnd).BeginInit();
            SuspendLayout();
            // 
            // volumeSlider1
            // 
            resources.ApplyResources(volumeSlider1, "volumeSlider1");
            volumeSlider1.Name = "volumeSlider1";
            volumeSlider1.VolumeChanged += VolumeSlider1_VolumeChanged;
            // 
            // button_Play
            // 
            resources.ApplyResources(button_Play, "button_Play");
            button_Play.Name = "button_Play";
            button_Play.UseVisualStyleBackColor = true;
            button_Play.Click += Button_Play_Click;
            // 
            // button_Stop
            // 
            resources.ApplyResources(button_Stop, "button_Stop");
            button_Stop.Name = "button_Stop";
            button_Stop.UseVisualStyleBackColor = true;
            button_Stop.Click += Button_Stop_Click;
            // 
            // button_Next
            // 
            resources.ApplyResources(button_Next, "button_Next");
            button_Next.Name = "button_Next";
            button_Next.UseVisualStyleBackColor = true;
            button_Next.Click += Button_Next_Click;
            // 
            // button_Prev
            // 
            resources.ApplyResources(button_Prev, "button_Prev");
            button_Prev.Name = "button_Prev";
            button_Prev.UseVisualStyleBackColor = true;
            button_Prev.Click += Button_Prev_Click;
            // 
            // timer_Reload
            // 
            timer_Reload.Tick += Timer_Reload_Tick;
            // 
            // label_Length
            // 
            resources.ApplyResources(label_Length, "label_Length");
            label_Length.Name = "label_Length";
            // 
            // label_Plength
            // 
            resources.ApplyResources(label_Plength, "label_Plength");
            label_Plength.Name = "label_Plength";
            // 
            // label_Samples
            // 
            resources.ApplyResources(label_Samples, "label_Samples");
            label_Samples.Name = "label_Samples";
            // 
            // label_Psamples
            // 
            resources.ApplyResources(label_Psamples, "label_Psamples");
            label_Psamples.Name = "label_Psamples";
            // 
            // label_File
            // 
            resources.ApplyResources(label_File, "label_File");
            label_File.Name = "label_File";
            // 
            // numericUpDown_LoopStart
            // 
            resources.ApplyResources(numericUpDown_LoopStart, "numericUpDown_LoopStart");
            numericUpDown_LoopStart.Name = "numericUpDown_LoopStart";
            numericUpDown_LoopStart.ValueChanged += NumericUpDown_LoopStart_ValueChanged;
            // 
            // numericUpDown_LoopEnd
            // 
            resources.ApplyResources(numericUpDown_LoopEnd, "numericUpDown_LoopEnd");
            numericUpDown_LoopEnd.Name = "numericUpDown_LoopEnd";
            numericUpDown_LoopEnd.ValueChanged += NumericUpDown_LoopEnd_ValueChanged;
            // 
            // button_LS_Current
            // 
            resources.ApplyResources(button_LS_Current, "button_LS_Current");
            button_LS_Current.Name = "button_LS_Current";
            button_LS_Current.UseVisualStyleBackColor = true;
            button_LS_Current.Click += Button_LS_Current_Click;
            // 
            // button_LE_Current
            // 
            resources.ApplyResources(button_LE_Current, "button_LE_Current");
            button_LE_Current.Name = "button_LE_Current";
            button_LE_Current.UseVisualStyleBackColor = true;
            button_LE_Current.Click += Button_LE_Current_Click;
            // 
            // button_SetStart
            // 
            resources.ApplyResources(button_SetStart, "button_SetStart");
            button_SetStart.Name = "button_SetStart";
            button_SetStart.UseVisualStyleBackColor = true;
            button_SetStart.Click += Button_SetStart_Click;
            // 
            // button_SetEnd
            // 
            resources.ApplyResources(button_SetEnd, "button_SetEnd");
            button_SetEnd.Name = "button_SetEnd";
            button_SetEnd.UseVisualStyleBackColor = true;
            button_SetEnd.Click += Button_SetEnd_Click;
            // 
            // label_LoopStart
            // 
            resources.ApplyResources(label_LoopStart, "label_LoopStart");
            label_LoopStart.Name = "label_LoopStart";
            // 
            // label_LoopEnd
            // 
            resources.ApplyResources(label_LoopEnd, "label_LoopEnd");
            label_LoopEnd.Name = "label_LoopEnd";
            // 
            // customTrackBar_LoopEnd
            // 
            resources.ApplyResources(customTrackBar_LoopEnd, "customTrackBar_LoopEnd");
            customTrackBar_LoopEnd.BackgroundColor = SystemColors.Control;
            customTrackBar_LoopEnd.DraggedThumbColor = Color.DarkRed;
            customTrackBar_LoopEnd.Maximum = 100;
            customTrackBar_LoopEnd.Minimum = 0;
            customTrackBar_LoopEnd.Name = "customTrackBar_LoopEnd";
            customTrackBar_LoopEnd.Orientation = Orientation.Horizontal;
            customTrackBar_LoopEnd.Shape = src.Controls.CustomTrackBar.ThumbShape.DownArrow;
            customTrackBar_LoopEnd.ShowLPCSamples = true;
            customTrackBar_LoopEnd.ShowTicks = true;
            customTrackBar_LoopEnd.ThumbColor = Color.Red;
            customTrackBar_LoopEnd.ThumbHeight = 25;
            customTrackBar_LoopEnd.ThumbSize = 10;
            customTrackBar_LoopEnd.ThumbWidth = 10;
            customTrackBar_LoopEnd.TickColor = Color.Black;
            customTrackBar_LoopEnd.TickFrequency = 10;
            customTrackBar_LoopEnd.TickPos = src.Controls.CustomTrackBar.TickPosition.Below;
            customTrackBar_LoopEnd.TickSize = 5;
            customTrackBar_LoopEnd.TrackColor = Color.Gray;
            customTrackBar_LoopEnd.TrackThickness = 8;
            customTrackBar_LoopEnd.Value = 0;
            // 
            // customTrackBar_Main
            // 
            resources.ApplyResources(customTrackBar_Main, "customTrackBar_Main");
            customTrackBar_Main.BackgroundColor = SystemColors.Control;
            customTrackBar_Main.DraggedThumbColor = Color.DarkBlue;
            customTrackBar_Main.Maximum = 100;
            customTrackBar_Main.Minimum = 0;
            customTrackBar_Main.Name = "customTrackBar_Main";
            customTrackBar_Main.Orientation = Orientation.Horizontal;
            customTrackBar_Main.Shape = src.Controls.CustomTrackBar.ThumbShape.Circle;
            customTrackBar_Main.ShowLPCSamples = true;
            customTrackBar_Main.ShowTicks = true;
            customTrackBar_Main.ThumbColor = Color.RoyalBlue;
            customTrackBar_Main.ThumbHeight = 67;
            customTrackBar_Main.ThumbSize = 10;
            customTrackBar_Main.ThumbWidth = 5;
            customTrackBar_Main.TickColor = Color.Black;
            customTrackBar_Main.TickFrequency = 10;
            customTrackBar_Main.TickPos = src.Controls.CustomTrackBar.TickPosition.Both;
            customTrackBar_Main.TickSize = 10;
            customTrackBar_Main.TrackColor = Color.Gray;
            customTrackBar_Main.TrackThickness = 40;
            customTrackBar_Main.Value = 0;
            // 
            // customTrackBar_LoopStart
            // 
            resources.ApplyResources(customTrackBar_LoopStart, "customTrackBar_LoopStart");
            customTrackBar_LoopStart.BackgroundColor = SystemColors.Control;
            customTrackBar_LoopStart.DraggedThumbColor = Color.DarkGreen;
            customTrackBar_LoopStart.Maximum = 100;
            customTrackBar_LoopStart.Minimum = 0;
            customTrackBar_LoopStart.Name = "customTrackBar_LoopStart";
            customTrackBar_LoopStart.Orientation = Orientation.Horizontal;
            customTrackBar_LoopStart.Shape = src.Controls.CustomTrackBar.ThumbShape.UpArrow;
            customTrackBar_LoopStart.ShowLPCSamples = true;
            customTrackBar_LoopStart.ShowTicks = true;
            customTrackBar_LoopStart.ThumbColor = Color.Green;
            customTrackBar_LoopStart.ThumbHeight = 35;
            customTrackBar_LoopStart.ThumbSize = 10;
            customTrackBar_LoopStart.ThumbWidth = 11;
            customTrackBar_LoopStart.TickColor = Color.Black;
            customTrackBar_LoopStart.TickFrequency = 10;
            customTrackBar_LoopStart.TickPos = src.Controls.CustomTrackBar.TickPosition.Above;
            customTrackBar_LoopStart.TickSize = 10;
            customTrackBar_LoopStart.TrackColor = Color.Gray;
            customTrackBar_LoopStart.TrackThickness = 8;
            customTrackBar_LoopStart.Value = 0;
            // 
            // label_main
            // 
            resources.ApplyResources(label_main, "label_main");
            label_main.Name = "label_main";
            // 
            // label_lstart
            // 
            resources.ApplyResources(label_lstart, "label_lstart");
            label_lstart.Name = "label_lstart";
            // 
            // label_lend
            // 
            resources.ApplyResources(label_lend, "label_lend");
            label_lend.Name = "label_lend";
            // 
            // label_StartTweak
            // 
            resources.ApplyResources(label_StartTweak, "label_StartTweak");
            label_StartTweak.Name = "label_StartTweak";
            // 
            // label_EndTweak
            // 
            resources.ApplyResources(label_EndTweak, "label_EndTweak");
            label_EndTweak.Name = "label_EndTweak";
            // 
            // label_Volume
            // 
            resources.ApplyResources(label_Volume, "label_Volume");
            label_Volume.Name = "label_Volume";
            // 
            // label_FileSize
            // 
            resources.ApplyResources(label_FileSize, "label_FileSize");
            label_FileSize.Name = "label_FileSize";
            // 
            // FormAudio
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            ControlBox = false;
            Controls.Add(label_FileSize);
            Controls.Add(label_Volume);
            Controls.Add(label_EndTweak);
            Controls.Add(label_StartTweak);
            Controls.Add(label_lend);
            Controls.Add(label_lstart);
            Controls.Add(label_main);
            Controls.Add(customTrackBar_Main);
            Controls.Add(label_LoopEnd);
            Controls.Add(label_LoopStart);
            Controls.Add(button_SetEnd);
            Controls.Add(button_SetStart);
            Controls.Add(button_LE_Current);
            Controls.Add(button_LS_Current);
            Controls.Add(numericUpDown_LoopEnd);
            Controls.Add(numericUpDown_LoopStart);
            Controls.Add(label_File);
            Controls.Add(label_Psamples);
            Controls.Add(label_Samples);
            Controls.Add(label_Plength);
            Controls.Add(label_Length);
            Controls.Add(button_Prev);
            Controls.Add(button_Next);
            Controls.Add(button_Stop);
            Controls.Add(button_Play);
            Controls.Add(volumeSlider1);
            Controls.Add(customTrackBar_LoopStart);
            Controls.Add(customTrackBar_LoopEnd);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormAudio";
            FormClosed += FormAudio_FormClosed;
            Load += FormAudio_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown_LoopStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_LoopEnd).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NAudio.Gui.VolumeSlider volumeSlider1;
        private Button button_Play;
        private Button button_Stop;
        private Button button_Next;
        private Button button_Prev;
        private System.Windows.Forms.Timer timer_Reload;
        private Label label_Length;
        private Label label_Plength;
        private Label label_Samples;
        private Label label_Psamples;
        private Label label_File;
        private Label label_LoopStart;
        private Label label_LoopEnd;
        internal NumericUpDown numericUpDown_LoopStart;
        internal NumericUpDown numericUpDown_LoopEnd;
        internal Button button_LS_Current;
        internal Button button_LE_Current;
        internal Button button_SetStart;
        internal Button button_SetEnd;
        private src.Controls.CustomTrackBar customTrackBar_Main;
        internal src.Controls.CustomTrackBar customTrackBar_LoopEnd;
        internal src.Controls.CustomTrackBar customTrackBar_LoopStart;
        private Label label_main;
        private Label label_lstart;
        private Label label_lend;
        private Label label1;
        private Label label2;
        private Label label_Volume;
        private Label label_FileSize;
        internal Label label_StartTweak;
        internal Label label_EndTweak;
    }
}