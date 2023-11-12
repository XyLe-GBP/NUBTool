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
            volumeSlider1 = new NAudio.Gui.VolumeSlider();
            trackBar_Main = new TrackBar();
            trackBar_LoopEnd = new TrackBar();
            trackBar_LoopStart = new TrackBar();
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
            ((System.ComponentModel.ISupportInitialize)trackBar_Main).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_LoopEnd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_LoopStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_LoopStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_LoopEnd).BeginInit();
            SuspendLayout();
            // 
            // volumeSlider1
            // 
            volumeSlider1.Location = new Point(692, 422);
            volumeSlider1.Name = "volumeSlider1";
            volumeSlider1.Size = new Size(96, 16);
            volumeSlider1.TabIndex = 0;
            volumeSlider1.VolumeChanged += VolumeSlider1_VolumeChanged;
            // 
            // trackBar_Main
            // 
            trackBar_Main.Location = new Point(12, 167);
            trackBar_Main.Name = "trackBar_Main";
            trackBar_Main.Size = new Size(776, 45);
            trackBar_Main.TabIndex = 1;
            // 
            // trackBar_LoopEnd
            // 
            trackBar_LoopEnd.Enabled = false;
            trackBar_LoopEnd.Location = new Point(12, 116);
            trackBar_LoopEnd.Name = "trackBar_LoopEnd";
            trackBar_LoopEnd.Size = new Size(776, 45);
            trackBar_LoopEnd.TabIndex = 2;
            // 
            // trackBar_LoopStart
            // 
            trackBar_LoopStart.Enabled = false;
            trackBar_LoopStart.Location = new Point(12, 218);
            trackBar_LoopStart.Name = "trackBar_LoopStart";
            trackBar_LoopStart.Size = new Size(776, 45);
            trackBar_LoopStart.TabIndex = 3;
            // 
            // button_Play
            // 
            button_Play.Location = new Point(282, 269);
            button_Play.Name = "button_Play";
            button_Play.Size = new Size(120, 45);
            button_Play.TabIndex = 4;
            button_Play.Text = "Play";
            button_Play.UseVisualStyleBackColor = true;
            button_Play.Click += Button_Play_Click;
            // 
            // button_Stop
            // 
            button_Stop.Location = new Point(408, 269);
            button_Stop.Name = "button_Stop";
            button_Stop.Size = new Size(120, 45);
            button_Stop.TabIndex = 6;
            button_Stop.Text = "Stop";
            button_Stop.UseVisualStyleBackColor = true;
            button_Stop.Click += Button_Stop_Click;
            // 
            // button_Next
            // 
            button_Next.Enabled = false;
            button_Next.Location = new Point(713, 269);
            button_Next.Name = "button_Next";
            button_Next.Size = new Size(75, 23);
            button_Next.TabIndex = 7;
            button_Next.Text = "Next";
            button_Next.UseVisualStyleBackColor = true;
            button_Next.Click += Button_Next_Click;
            // 
            // button_Prev
            // 
            button_Prev.Enabled = false;
            button_Prev.Location = new Point(12, 269);
            button_Prev.Name = "button_Prev";
            button_Prev.Size = new Size(75, 23);
            button_Prev.TabIndex = 8;
            button_Prev.Text = "Preview";
            button_Prev.UseVisualStyleBackColor = true;
            button_Prev.Click += Button_Prev_Click;
            // 
            // timer_Reload
            // 
            timer_Reload.Tick += Timer_Reload_Tick;
            // 
            // label_Length
            // 
            label_Length.AutoSize = true;
            label_Length.Location = new Point(12, 295);
            label_Length.Name = "label_Length";
            label_Length.Size = new Size(47, 15);
            label_Length.TabIndex = 9;
            label_Length.Text = "Length:";
            // 
            // label_Plength
            // 
            label_Plength.AutoSize = true;
            label_Plength.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label_Plength.Location = new Point(12, 310);
            label_Plength.Name = "label_Plength";
            label_Plength.Size = new Size(88, 25);
            label_Plength.TabIndex = 10;
            label_Plength.Text = "00:00:00";
            // 
            // label_Samples
            // 
            label_Samples.AutoSize = true;
            label_Samples.Location = new Point(12, 337);
            label_Samples.Name = "label_Samples";
            label_Samples.Size = new Size(53, 15);
            label_Samples.TabIndex = 11;
            label_Samples.Text = "Samples:";
            // 
            // label_Psamples
            // 
            label_Psamples.AutoSize = true;
            label_Psamples.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label_Psamples.Location = new Point(12, 352);
            label_Psamples.Name = "label_Psamples";
            label_Psamples.Size = new Size(100, 25);
            label_Psamples.TabIndex = 12;
            label_Psamples.Text = "00000000";
            // 
            // label_File
            // 
            label_File.Location = new Point(534, 295);
            label_File.Name = "label_File";
            label_File.Size = new Size(254, 15);
            label_File.TabIndex = 13;
            label_File.Text = "FileName";
            label_File.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numericUpDown_LoopStart
            // 
            numericUpDown_LoopStart.Enabled = false;
            numericUpDown_LoopStart.Location = new Point(282, 320);
            numericUpDown_LoopStart.Name = "numericUpDown_LoopStart";
            numericUpDown_LoopStart.Size = new Size(120, 23);
            numericUpDown_LoopStart.TabIndex = 14;
            numericUpDown_LoopStart.ValueChanged += NumericUpDown_LoopStart_ValueChanged;
            // 
            // numericUpDown_LoopEnd
            // 
            numericUpDown_LoopEnd.Enabled = false;
            numericUpDown_LoopEnd.Location = new Point(408, 320);
            numericUpDown_LoopEnd.Name = "numericUpDown_LoopEnd";
            numericUpDown_LoopEnd.Size = new Size(120, 23);
            numericUpDown_LoopEnd.TabIndex = 15;
            numericUpDown_LoopEnd.ValueChanged += NumericUpDown_LoopEnd_ValueChanged;
            // 
            // button_LS_Current
            // 
            button_LS_Current.Enabled = false;
            button_LS_Current.Location = new Point(198, 349);
            button_LS_Current.Name = "button_LS_Current";
            button_LS_Current.Size = new Size(204, 40);
            button_LS_Current.TabIndex = 16;
            button_LS_Current.Text = "Set start position at current location";
            button_LS_Current.UseVisualStyleBackColor = true;
            button_LS_Current.Click += Button_LS_Current_Click;
            // 
            // button_LE_Current
            // 
            button_LE_Current.Enabled = false;
            button_LE_Current.Location = new Point(408, 349);
            button_LE_Current.Name = "button_LE_Current";
            button_LE_Current.Size = new Size(204, 40);
            button_LE_Current.TabIndex = 17;
            button_LE_Current.Text = "Set end position at current location";
            button_LE_Current.UseVisualStyleBackColor = true;
            button_LE_Current.Click += Button_LE_Current_Click;
            // 
            // button_SetStart
            // 
            button_SetStart.Enabled = false;
            button_SetStart.Location = new Point(282, 395);
            button_SetStart.Name = "button_SetStart";
            button_SetStart.Size = new Size(120, 23);
            button_SetStart.TabIndex = 18;
            button_SetStart.Text = "Set LoopStart";
            button_SetStart.UseVisualStyleBackColor = true;
            button_SetStart.Click += Button_SetStart_Click;
            // 
            // button_SetEnd
            // 
            button_SetEnd.Enabled = false;
            button_SetEnd.Location = new Point(408, 395);
            button_SetEnd.Name = "button_SetEnd";
            button_SetEnd.Size = new Size(120, 23);
            button_SetEnd.TabIndex = 19;
            button_SetEnd.Text = "Set LoopEnd";
            button_SetEnd.UseVisualStyleBackColor = true;
            button_SetEnd.Click += Button_SetEnd_Click;
            // 
            // label_LoopStart
            // 
            label_LoopStart.AutoSize = true;
            label_LoopStart.Location = new Point(12, 395);
            label_LoopStart.Name = "label_LoopStart";
            label_LoopStart.Size = new Size(38, 15);
            label_LoopStart.TabIndex = 22;
            label_LoopStart.Text = "label3";
            // 
            // label_LoopEnd
            // 
            label_LoopEnd.AutoSize = true;
            label_LoopEnd.Location = new Point(12, 422);
            label_LoopEnd.Name = "label_LoopEnd";
            label_LoopEnd.Size = new Size(38, 15);
            label_LoopEnd.TabIndex = 23;
            label_LoopEnd.Text = "label4";
            // 
            // FormAudio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ControlBox = false;
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
            Controls.Add(trackBar_LoopStart);
            Controls.Add(trackBar_LoopEnd);
            Controls.Add(trackBar_Main);
            Controls.Add(volumeSlider1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormAudio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormAudio";
            FormClosed += FormAudio_FormClosed;
            Load += FormAudio_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar_Main).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_LoopEnd).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_LoopStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_LoopStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_LoopEnd).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NAudio.Gui.VolumeSlider volumeSlider1;
        private TrackBar trackBar_Main;
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
        internal TrackBar trackBar_LoopEnd;
        internal TrackBar trackBar_LoopStart;
        internal NumericUpDown numericUpDown_LoopStart;
        internal NumericUpDown numericUpDown_LoopEnd;
        internal Button button_LS_Current;
        internal Button button_LE_Current;
        internal Button button_SetStart;
        internal Button button_SetEnd;
    }
}