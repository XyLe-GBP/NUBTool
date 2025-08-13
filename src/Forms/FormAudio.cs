using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;
using System.Text;
using static NUBTool.Common;
using static NUBTool.Common.Generic;
using System.ComponentModel;

namespace NUBTool
{
    public partial class FormAudio : Form
    {
        FormMain FormMain;
        private readonly WaveInEvent wi = new();
        private readonly WaveOutEvent wo = new();
        WaveFileReader reader = null!;
        private OffsetSampleProvider osp = null!;
        //private AudioFileReader reader = null!;
        long Sample, Start = 0, End = 0;
        int bytePerSec, position, length, smplrate;
        uint btnpos;
        TimeSpan time;
        bool mouseDown = false, stopflag = false, IsPausedMoveTrackbar, SmoothSamples = false, IsPlaybackNUB = false, IsFirstNext = true;
        float ScaleWidthTrk = 0f, ScaleWidthStart = 0f, ScaleWidthEnd = 0f;
        Point MainDefaultPoint = new(25, 148), StartDefaultPoint = new(20, 236), EndDefaultPoint = new(20, 95);

        Point labelTrk, labelStart, labelEnd;

        private volatile bool SLTAlive;
        ThreadStart StartPlaybackThread_s;
        Thread Playback_s;

        int[] bufferloop = new int[2];

        private static FormAudio _formAudioInstance = null!;
        public static FormAudio FormAudioInstance
        {
            get
            {
                return _formAudioInstance;
            }
            set
            {
                _formAudioInstance = value;
            }
        }

        public string SampleLabel
        {
            get
            {
                return label_Psamples.Text;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LoopStartLabel
        {
            get
            {
                return label_LoopStart.Text;
            }
            set
            {
                label_LoopStart.Text = value;
            }
        }

        public long LoopStart
        {
            get
            {
                return Start;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LoopEndLabel
        {
            get
            {
                return label_LoopEnd.Text;
            }
            set
            {
                label_LoopEnd.Text = value;
            }
        }

        public long LoopEnd
        {
            get
            {
                return End;
            }
        }

        public int SampleRate
        {
            get
            {
                return reader.WaveFormat.SampleRate;
            }
        }

        public uint ButtonPosition
        {
            get
            {
                return btnpos;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int[] BufferLoopPosition
        {
            get
            {
                return bufferloop;
            }
            set
            {
                bufferloop = value;
            }
        }

        public FormAudio(FormMain formMain)
        {
            FormMain = formMain;
            InitializeComponent();

            labelTrk = MainDefaultPoint;
            labelStart = StartDefaultPoint;
            labelEnd = EndDefaultPoint;

            customTrackBar_Main.Scroll += customTrackBar_Main_Scroll;
            customTrackBar_LoopStart.Scroll += customTrackBar_LoopStart_Scroll;
            customTrackBar_LoopEnd.Scroll += customTrackBar_LoopEnd_Scroll;
            customTrackBar_Main.MouseDown += customTrackBar_Main_MouseDown;
            customTrackBar_Main.MouseUp += customTrackBar_Main_MouseUp;
            //label_Samples.Text = Localization.SampleCaption + ":";
            label_Psamples.Text = "0";
            //label_Length.Text = Localization.LengthCaption + ":";
            label_Plength.Text = "00:00:00";
            label_LoopStart.Text = "";
            label_LoopEnd.Text = "";
            timer_Reload.Interval = 1;
        }

        private void customTrackBar_LoopEnd_Scroll(object? sender, EventArgs e)
        {
            numericUpDown_LoopEnd.Value = customTrackBar_LoopEnd.Value;
        }

        private void customTrackBar_LoopStart_Scroll(object? sender, EventArgs e)
        {
            numericUpDown_LoopStart.Value = customTrackBar_LoopStart.Value;
        }

        private void customTrackBar_Main_Scroll(object? sender, EventArgs e)
        {
            reader.CurrentTime = TimeSpan.FromMilliseconds(customTrackBar_Main.Value);
        }

        private void customTrackBar_Main_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!mouseDown) return;
            mouseDown = false;
        }

        private void customTrackBar_Main_MouseDown(object? sender, MouseEventArgs e)
        {
            if (reader == null) return;
            mouseDown = true;
        }

        private void FormAudio_Load(object sender, EventArgs e)
        {
            Config.Load(xmlpath);

            FormAudioInstance = this;

            switch (bool.Parse(Config.Entry["SmoothSamples"].Value))
            {
                case true:
                    SmoothSamples = true;
                    break;
                case false:
                    SmoothSamples = false;
                    break;
            }

            /*switch (bool.Parse(Config.Entry["PlaybackNUB"].Value))
            {
                case true:
                    IsPlaybackNUB = true;
                    break;
                case false:
                    IsPlaybackNUB = false;
                    break;
            }*/

            if (SmoothSamples)
            {
                wo.DesiredLatency = 200; // 250
                wo.NumberOfBuffers = 16; // 8
            }
            else
            {
                wo.DesiredLatency = 300; // 250
                wo.NumberOfBuffers = 3; // 8
            }

            Generic.IsAudioStreamingReloaded = true;
            if (OpenFilePaths.Length == 1)
            {
                reader = new(OpenFilePaths[0]);
                var fs = File.OpenRead(OpenFilePaths[0]);
                byte[] by = new byte[fs.Length];
                fs.Read(by, 0, by.Length);
                fs.Close();
                //FileInfo fi = new(OpenFilePaths[0]);
                FileInfo fi = new(OriginalPaths[0]);
                label_FileSize.Text = (fi.Length / 1024).ToString() + " KiB (" + fi.Length.ToString() + " Bytes)" + " [STREAM_TOTAL: " + Utils.GetStreamTotals(fi.FullName) + " Bytes]";
                label_File.Text = fi.Name + " [" + reader.WaveFormat.BitsPerSample + "-bit," + reader.WaveFormat.SampleRate + "Hz]";
                button_Prev.Enabled = false;
                button_Next.Enabled = false;
            }
            else
            {
                reader = new(OpenFilePaths[0]);
                //RSS = new RawSourceWaveStream(reader, reader.WaveFormat);
                //FileInfo fi = new(OpenFilePaths[0]);
                FileInfo fi = new(OriginalPaths[0]);
                label_FileSize.Text = (fi.Length / 1024).ToString() + " KiB (" + fi.Length.ToString() + " Bytes)" + " [STREAM_TOTAL: " + Utils.GetStreamTotals(fi.FullName) + " Bytes]";
                label_File.Text = fi.Name + " [" + reader.WaveFormat.BitsPerSample + "-bit," + reader.WaveFormat.SampleRate + "Hz]";
                button_Prev.Enabled = false;
                button_Next.Enabled = true;
                btnpos = 1;
            }
            _ = FormMain.FormMainInstance.Meta;

            wo.Init(new WaveChannel32(reader));
            Generic.IsAudioStreamingReloaded = false;
            customTrackBar_Main.Minimum = 0;
            customTrackBar_Main.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            customTrackBar_LoopStart.Minimum = 0;
            customTrackBar_LoopStart.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            customTrackBar_LoopEnd.Minimum = 0;
            customTrackBar_LoopEnd.Maximum = (int)reader.TotalTime.TotalMilliseconds;

            ScaleWidthTrk = (float)customTrackBar_Main.Size.Width / ((float)customTrackBar_Main.Maximum - (float)customTrackBar_Main.Minimum);
            ScaleWidthStart = (float)customTrackBar_LoopStart.Size.Width / ((float)customTrackBar_LoopStart.Maximum - (float)customTrackBar_LoopStart.Minimum);
            ScaleWidthEnd = (float)customTrackBar_LoopEnd.Size.Width / ((float)customTrackBar_LoopEnd.Maximum - (float)customTrackBar_LoopEnd.Minimum);

            customTrackBar_Main.TickFrequency = 1000;
            customTrackBar_LoopStart.TickFrequency = 1000;
            customTrackBar_LoopEnd.TickFrequency = 1000;
            numericUpDown_LoopStart.Minimum = 0;
            numericUpDown_LoopStart.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            numericUpDown_LoopStart.Increment = 1;
            numericUpDown_LoopEnd.Minimum = 0;
            numericUpDown_LoopEnd.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            numericUpDown_LoopEnd.Increment = 1;
            wo.Volume = volumeSlider1.Volume;

            label_main.Text = "";
            label_lstart.Text = "";
            label_lend.Text = "";

            int tb = (int)reader.TotalTime.TotalMilliseconds / 2;
            numericUpDown_LoopEnd.Value = tb;

            SetLoopPointsWithNUBBuffer(reader.WaveFormat.SampleRate, 0);
            //SetLoopPointWithMultipleFiles(reader.WaveFormat.SampleRate, btnpos);
            Generic.AudioTotalSamples = reader.SampleCount;
        }

        private void Button_Prev_Click(object sender, EventArgs e)
        {
            btnpos--;

            if (btnpos - 1 == uint.MaxValue) {
                btnpos++;
            }

            Debug.WriteLine("Prev btnpos: ", string.Format("{0}", btnpos));
            Debug.WriteLine("MultipleFilesLoopOKFlags[]: ", string.Join(", ", MultipleFilesLoopOKFlags));
            Debug.WriteLine("MultipleLoopStarts[]: ", string.Join(", ", MultipleLoopStarts));
            Debug.WriteLine("MultipleLoopEnds[]: ", string.Join(", ", MultipleLoopEnds));

            if (!MultipleFilesLoopOKFlags[btnpos] && IsLoopWarning && IsOpenMulti && FormMain.checkBox_LoopEnable.Checked)
            {
                if (MultipleLoopStarts[btnpos] == 0 || MultipleLoopEnds[btnpos] == 0)
                {
                    DialogResult dr = MessageBox.Show(Localizable.Localization.LoopWarningCaption, Localizable.Localization.MSGBoxWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        if (btnpos != OpenFilePaths.Length)
                        {
                            btnpos++;
                        }

                        return;
                    }
                }
            }

            Generic.IsAudioStreamingReloaded = true;



            //FileInfo fi = new(Common.Generic.OpenFilePaths[btnpos - 1]);]
            FileInfo fi = new(OriginalPaths[btnpos - 1]);
            label_FileSize.Text = (fi.Length / 1024).ToString() + " KiB (" + fi.Length.ToString() + " Bytes)" + " [STREAM_TOTAL: " + Utils.GetStreamTotals(fi.FullName) + " Bytes]";
            label_File.Text = fi.Name + " [" + reader.WaveFormat.BitsPerSample + "-bit," + reader.WaveFormat.SampleRate + "Hz]";

            wo.Stop();
            button_Play.Text = Localizable.Localization.PlayCaption;
            reader.Position = 0;
            reader.Close();
            button_Stop.Enabled = false;

            _ = FormMain.FormMainInstance.Meta;

            if (btnpos == 1)
            {
                reader = new(Common.Generic.OpenFilePaths[btnpos - 1]);
                wo.Init(new WaveChannel32(reader));
                ResetAFR();
                button_Prev.Enabled = false;
                button_Next.Enabled = true;
            }
            else
            {
                reader = new(Common.Generic.OpenFilePaths[btnpos - 1]);
                wo.Init(new WaveChannel32(reader));
                ResetAFR();
                button_Prev.Enabled = true;
                button_Next.Enabled = true;
            }
            SetLoopPointsWithNUBBuffer(reader.WaveFormat.SampleRate, btnpos - 1);
            SetLoopPointWithMultipleFiles(reader.WaveFormat.SampleRate, btnpos - 1);
            Generic.IsAudioStreamingReloaded = false;
        }

        private void Button_Next_Click(object sender, EventArgs e)
        {
            btnpos++;

            if (btnpos == OpenFilePaths.Length + 1) {
                btnpos--;
            }

            Debug.WriteLine("Next btnpos: ", string.Format("{0}", btnpos));
            Debug.WriteLine("MultipleFilesLoopOKFlags[]: ", string.Join(", ", MultipleFilesLoopOKFlags));
            Debug.WriteLine("MultipleLoopStarts[]: ", string.Join(", ", MultipleLoopStarts));
            Debug.WriteLine("MultipleLoopEnds[]: ", string.Join(", ", MultipleLoopEnds));

            if (!MultipleFilesLoopOKFlags[btnpos - 2] && IsLoopWarning && IsOpenMulti && FormMain.checkBox_LoopEnable.Checked)
            {
                if (MultipleLoopStarts[btnpos - 2] == 0 || MultipleLoopEnds[btnpos - 2] == 0)
                {
                    DialogResult dr = MessageBox.Show(Localizable.Localization.LoopWarningCaption, Localizable.Localization.MSGBoxWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        if (btnpos != 1)
                        {
                            btnpos--;
                        }
                        return;
                    }
                }
            }

            Generic.IsAudioStreamingReloaded = true;



            // FileInfo fi = new(Common.Generic.OpenFilePaths[btnpos - 1]);
            FileInfo fi = new(OriginalPaths[btnpos - 1]);
            label_FileSize.Text = (fi.Length / 1024).ToString() + " KiB (" + fi.Length.ToString() + " Bytes)" + " [STREAM_TOTAL: " + Utils.GetStreamTotals(fi.FullName) + " Bytes]";
            label_File.Text = fi.Name + " [" + reader.WaveFormat.BitsPerSample + "-bit," + reader.WaveFormat.SampleRate + "Hz]";

            wo.Stop();
            button_Play.Text = Localizable.Localization.PlayCaption;
            reader.Position = 0;
            reader.Close();
            button_Stop.Enabled = false;

            _ = FormMain.FormMainInstance.Meta;

            if (btnpos == Common.Generic.OpenFilePaths.Length)
            {
                reader = new(Common.Generic.OpenFilePaths[btnpos - 1]);
                wo.Init(new WaveChannel32(reader));
                ResetAFR();
                button_Next.Enabled = false;
                button_Prev.Enabled = true;
            }
            else
            {
                reader = new(Common.Generic.OpenFilePaths[btnpos - 1]);
                wo.Init(new WaveChannel32(reader));
                ResetAFR();
                button_Next.Enabled = true;
                button_Prev.Enabled = true;
            }
            SetLoopPointsWithNUBBuffer(reader.WaveFormat.SampleRate, btnpos - 1);
            SetLoopPointWithMultipleFiles(reader.WaveFormat.SampleRate, btnpos - 1);

            Generic.IsAudioStreamingReloaded = false;
        }

        private void NumericUpDown_LoopStart_ValueChanged(object sender, EventArgs e)
        {
            customTrackBar_LoopStart.Value = (int)numericUpDown_LoopStart.Value;
        }

        private void NumericUpDown_LoopEnd_ValueChanged(object sender, EventArgs e)
        {
            customTrackBar_LoopEnd.Value = (int)numericUpDown_LoopEnd.Value;
        }

        private void FormAudio_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (timer_Reload.Enabled == true) timer_Reload.Enabled = false;
            reader.Position = 0;
            //wi.Dispose();
            wo.Stop();
            wo.Dispose();
            reader.Close();
            reader = null!;
        }

        private void VolumeSlider1_VolumeChanged(object sender, EventArgs e)
        {
            wo.Volume = volumeSlider1.Volume;
        }

        private void Button_LS_Current_Click(object sender, EventArgs e)
        {
            customTrackBar_LoopStart.Value = customTrackBar_Main.Value;
            numericUpDown_LoopStart.Value = customTrackBar_LoopStart.Value;
        }

        private void Button_LE_Current_Click(object sender, EventArgs e)
        {
            customTrackBar_LoopEnd.Value = customTrackBar_Main.Value;
            numericUpDown_LoopEnd.Value = customTrackBar_LoopEnd.Value;
        }

        private void Button_SetStart_Click(object sender, EventArgs e)
        {
            long pos;
            TimeSpan oldc = reader.CurrentTime;
            reader.CurrentTime = TimeSpan.FromMilliseconds(customTrackBar_LoopStart.Value);
            pos = reader.Position / reader.WaveFormat.BlockAlign;
            reader.CurrentTime = oldc;
            label_LoopStart.Text = "LoopStart: " + pos.ToString() + " " + Localizable.Localization.SampleCaption;
            Start = pos;
            FormMain.textBox_LoopStart.Text = pos.ToString();
            if (IsOpenMulti)
            {
                MultipleLoopStarts[btnpos - 1] = (int)pos;
                if (MultipleLoopEnds[btnpos - 1] != 0)
                {
                    MultipleFilesLoopOKFlags[btnpos - 1] = true;
                }
                else
                {
                    MultipleFilesLoopOKFlags[btnpos - 1] = false;
                }
            }
            else
            {
                MultipleLoopStarts[0] = (int)pos;
                if (MultipleLoopEnds[0] != 0)
                {
                    MultipleFilesLoopOKFlags[0] = true;
                    LoopStartNG = false;
                }
                else
                {
                    MultipleFilesLoopOKFlags[0] = false;
                    if (string.IsNullOrWhiteSpace(FormMain.FormMainInstance.textBox_LoopStart.Text))
                    {
                        LoopStartNG = true;
                    }
                    else
                    {
                        LoopStartNG = false;
                    }
                }
                Debug.WriteLine("MultipleFilesLoopOKFlags[]: ", string.Join(", ", MultipleFilesLoopOKFlags));
                Debug.WriteLine("[Flag] LoopStartNG: ", string.Join(", ", LoopStartNG));
            }
        }

        private void Button_SetEnd_Click(object sender, EventArgs e)
        {
            long pos;
            TimeSpan oldc = reader.CurrentTime;
            reader.CurrentTime = TimeSpan.FromMilliseconds(customTrackBar_LoopEnd.Value);
            pos = reader.Position / reader.WaveFormat.BlockAlign;
            reader.CurrentTime = oldc;
            label_LoopEnd.Text = "LoopEnd: " + pos.ToString() + " " + Localizable.Localization.SampleCaption;
            End = pos;
            FormMain.textBox_LoopEnd.Text = pos.ToString();
            if (IsOpenMulti)
            {
                MultipleLoopEnds[btnpos - 1] = (int)pos;
                if (MultipleLoopStarts[btnpos - 1] != 0)
                {
                    MultipleFilesLoopOKFlags[btnpos - 1] = true;
                }
                else
                {
                    label_LoopEnd.Text = string.Empty;
                    MultipleFilesLoopOKFlags[btnpos - 1] = false;
                }
            }
            else
            {
                MultipleLoopEnds[0] = (int)pos;
                if (MultipleLoopStarts[0] != 0)
                {
                    MultipleFilesLoopOKFlags[0] = true;
                    LoopEndNG = false;
                }
                else
                {
                    label_LoopEnd.Text = string.Empty;
                    MultipleFilesLoopOKFlags[0] = false;
                    if (string.IsNullOrWhiteSpace(FormMain.FormMainInstance.textBox_LoopEnd.Text))
                    {
                        LoopEndNG = true;
                    }
                    else
                    {
                        LoopEndNG = false;
                    }
                }
                Debug.WriteLine("MultipleFilesLoopOKFlags[]: ", string.Join(", ", MultipleFilesLoopOKFlags));
                Debug.WriteLine("[Flag] LoopEndNG: ", string.Join(", ", LoopEndNG));
            }
        }

        private async void Button_Play_Click(object sender, EventArgs e)
        {
            switch (wo.PlaybackState)
            {
                case PlaybackState.Stopped:
                    bytePerSec = reader.WaveFormat.BitsPerSample / 8 * reader.WaveFormat.SampleRate * reader.WaveFormat.Channels;
                    length = (int)reader.Length / bytePerSec;

                    stopflag = false;
                    timer_Reload.Enabled = true;
                    wo.Play();
                    button_Play.Text = Localizable.Localization.PauseCaption;
                    await Task.Run(Playback);
                    button_Stop.Enabled = true;
                    break;
                case PlaybackState.Paused:
                    if (IsPausedMoveTrackbar)
                    {
                        wo.Stop();
                        reader.CurrentTime = TimeSpan.FromMilliseconds(customTrackBar_Main.Value);
                        wo.Play();
                        await Task.Run(Playback);
                        IsPausedMoveTrackbar = false;
                    }
                    else
                    {
                        wo.Play();
                    }

                    button_Play.Text = Localizable.Localization.PauseCaption;
                    break;
                case PlaybackState.Playing:
                    wo.Pause();
                    button_Play.Text = Localizable.Localization.PlayCaption;
                    break;
            }
        }

        private void Button_Stop_Click(object sender, EventArgs e)
        {
            if (wo.PlaybackState != PlaybackState.Stopped)
            {
                stopflag = true;
                timer_Reload.Stop();
                wo.Stop();
                button_Play.Text = Localizable.Localization.PlayCaption;
                reader.Position = 0;
                button_Stop.Enabled = false;
                Resettrackbarlabels();
            }
        }

        private void Playback()
        {
            object lockobj = new();
            lock (lockobj)
            {
                SLTAlive = true;
                StartPlaybackThread_s = new(StartPlaybackThread);
                Playback_s = new(StartPlaybackThread_s)
                {
                    Name = "waveOut",
                    IsBackground = true,
                    Priority = ThreadPriority.AboveNormal
                };
                Playback_s.SetApartmentState(ApartmentState.STA);
                Playback_s.Start();
            }
        }

        private void StartPlaybackThread()
        {
            while (wo.PlaybackState != PlaybackState.Stopped)
            {
                if (!SLTAlive) { return; }
                position = (int)reader.Position / reader.WaveFormat.AverageBytesPerSecond;
                time = new(0, 0, position);
                Sample = reader.Position / reader.BlockAlign;
            }
        }

        private void Timer_Reload_Tick(object sender, EventArgs e)
        {
            if (!mouseDown) customTrackBar_Main.Value = (int)reader.CurrentTime.TotalMilliseconds;
            if (IsEnableLoop == true && reader.CurrentTime >= TimeSpan.FromMilliseconds(customTrackBar_LoopEnd.Value))
            {
                reader.CurrentTime = TimeSpan.FromMilliseconds(customTrackBar_LoopStart.Value);
                Sample = reader.Position / reader.BlockAlign;
            }
            if (reader.CurrentTime == reader.TotalTime)
            {
                stopflag = true;
                Sample = reader.SampleCount;
                wo.Stop();
                button_Play.Text = Localizable.Localization.PlayCaption;
                reader.Position = 0;
                button_Stop.Enabled = false;
                Resettrackbarlabels();
            }
            else if (reader.Position == 0 || customTrackBar_Main.Value == 0)
            {
                if (!stopflag)
                {
                    wo.Stop();
                    button_Stop.Enabled = false;
                    Sample = 0;
                    reader.Position = 0;
                    wo.Play();
                    button_Play.Text = Localizable.Localization.PauseCaption;
                    Task.Run(Playback);
                    button_Stop.Enabled = true;
                }
            }
            SetTrackbarTrack();
            SetTrackbarStart();
            SetTrackbarEnd();
            StringBuilder str = new(Sample.ToString());

            label_main.Text = customTrackBar_Main.Value.ToString();
            label_lstart.Text = customTrackBar_LoopStart.Value.ToString();
            label_lend.Text = customTrackBar_LoopEnd.Value.ToString();
            label_Length.Text = Localizable.Localization.LengthCaption + ":";
            label_Plength.Text = time.ToString(@"hh\:mm\:ss");

            label_Samples.Text = Localizable.Localization.SampleCaption + ":";
            label_Psamples.Text = str.ToString();
        }

        private void ResetAFR()
        {
            customTrackBar_Main.Minimum = 0;
            customTrackBar_Main.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            customTrackBar_LoopStart.Minimum = 0;
            customTrackBar_LoopStart.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            customTrackBar_LoopEnd.Minimum = 0;
            customTrackBar_LoopEnd.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            customTrackBar_Main.TickFrequency = 1000;
            customTrackBar_LoopStart.TickFrequency = 1000;
            customTrackBar_LoopEnd.TickFrequency = 1000;
            numericUpDown_LoopStart.Minimum = 0;
            numericUpDown_LoopStart.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            numericUpDown_LoopStart.Increment = 1;
            numericUpDown_LoopEnd.Minimum = 0;
            numericUpDown_LoopEnd.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            numericUpDown_LoopEnd.Increment = 1;
            wo.Volume = volumeSlider1.Volume;

            int tb = (int)reader.TotalTime.TotalMilliseconds / 2;
            numericUpDown_LoopStart.Value = 0;
            numericUpDown_LoopEnd.Value = tb;

            Generic.AudioTotalSamples = reader.SampleCount;

            Resettrackbarlabels();
        }

        private void Resettrackbarlabels()
        {
            ScaleWidthTrk = (float)customTrackBar_Main.Size.Width / ((float)customTrackBar_Main.Maximum - (float)customTrackBar_Main.Minimum);
            ScaleWidthStart = (float)customTrackBar_LoopStart.Size.Width / ((float)customTrackBar_LoopStart.Maximum - (float)customTrackBar_LoopStart.Minimum);
            ScaleWidthEnd = (float)customTrackBar_LoopEnd.Size.Width / ((float)customTrackBar_LoopEnd.Maximum - (float)customTrackBar_LoopEnd.Minimum);
            labelTrk = MainDefaultPoint;
            labelStart = StartDefaultPoint;
            labelEnd = EndDefaultPoint;
        }

        private void SetTrackbarTrack()
        {
            if (customTrackBar_Main.Value < customTrackBar_Main.Maximum / 2)
            {
                label_main.Location = new Point(labelTrk.X + (int)((customTrackBar_Main.Value - customTrackBar_Main.Minimum) * ScaleWidthTrk) - customTrackBar_Main.Location.X - labelTrk.X + 9, labelTrk.Y);
            }
            else if (customTrackBar_Main.Value > customTrackBar_Main.Maximum / 2)
            {
                label_main.Location = new Point(labelTrk.X + (int)((customTrackBar_Main.Value - customTrackBar_Main.Minimum) * ScaleWidthTrk) - customTrackBar_Main.Location.X - labelTrk.X - 9, labelTrk.Y);
            }
            else if (customTrackBar_Main.Value == customTrackBar_Main.Maximum / 2)
            {
                label_main.Location = new Point(labelTrk.X + (int)((customTrackBar_Main.Value - customTrackBar_Main.Minimum) * ScaleWidthTrk) - customTrackBar_Main.Location.X - labelTrk.X, labelTrk.Y);
            }
            else
            {
                label_main.Location = new Point(labelTrk.X + (int)((customTrackBar_Main.Value - customTrackBar_Main.Minimum) * ScaleWidthTrk) - customTrackBar_Main.Location.X - labelTrk.X, labelTrk.Y);
            }
        }

        private void SetTrackbarStart()
        {
            if (customTrackBar_LoopStart.Value < customTrackBar_LoopStart.Maximum / 2)
            {
                label_lstart.Location = new Point(labelStart.X + (int)((customTrackBar_LoopStart.Value - customTrackBar_LoopStart.Minimum) * ScaleWidthStart) - customTrackBar_LoopStart.Location.X - labelStart.X + 9, labelStart.Y);
            }
            else if (customTrackBar_LoopStart.Value > customTrackBar_LoopStart.Maximum / 2)
            {
                label_lstart.Location = new Point(labelStart.X + (int)((customTrackBar_LoopStart.Value - customTrackBar_LoopStart.Minimum) * ScaleWidthStart) - customTrackBar_LoopStart.Location.X - labelStart.X - 9, labelStart.Y);
            }
            else if (customTrackBar_LoopStart.Value == customTrackBar_LoopStart.Maximum / 2)
            {
                label_lstart.Location = new Point(labelStart.X + (int)((customTrackBar_LoopStart.Value - customTrackBar_LoopStart.Minimum) * ScaleWidthStart) - customTrackBar_LoopStart.Location.X - labelStart.X, labelStart.Y);
            }
            else
            {
                label_lstart.Location = new Point(labelStart.X + (int)((customTrackBar_LoopStart.Value - customTrackBar_LoopStart.Minimum) * ScaleWidthStart) - customTrackBar_LoopStart.Location.X - labelStart.X, labelStart.Y);
            }
        }

        private void SetTrackbarEnd()
        {
            if (customTrackBar_LoopEnd.Value < customTrackBar_LoopEnd.Maximum / 2)
            {
                label_lend.Location = new Point(labelEnd.X + (int)((customTrackBar_LoopEnd.Value - customTrackBar_LoopEnd.Minimum) * ScaleWidthEnd) - customTrackBar_LoopEnd.Location.X - labelEnd.X + 9, labelEnd.Y);
            }
            else if (customTrackBar_LoopEnd.Value > customTrackBar_LoopEnd.Maximum / 2)
            {
                label_lend.Location = new Point(labelEnd.X + (int)((customTrackBar_LoopEnd.Value - customTrackBar_LoopEnd.Minimum) * ScaleWidthEnd) - customTrackBar_LoopEnd.Location.X - labelEnd.X - 9, labelEnd.Y);
            }
            else if (customTrackBar_LoopEnd.Value == customTrackBar_LoopEnd.Maximum / 2)
            {
                label_lend.Location = new Point(labelEnd.X + (int)((customTrackBar_LoopEnd.Value - customTrackBar_LoopEnd.Minimum) * ScaleWidthEnd) - customTrackBar_LoopEnd.Location.X - labelEnd.X, labelEnd.Y);
            }
            else
            {
                label_lend.Location = new Point(labelEnd.X + (int)((customTrackBar_LoopEnd.Value - customTrackBar_LoopEnd.Minimum) * ScaleWidthEnd) - customTrackBar_LoopEnd.Location.X - labelEnd.X, labelEnd.Y);
            }
        }

        /// <summary>
        /// NUBからループ情報を読み取りUIに反映
        /// </summary>
        /// <param name="samplerate">サンプリング周波数</param>
        /// <param name="pos">Current ButtonPosition (multiple files only)</param>
        private void SetLoopPointsWithNUBBuffer(int samplerate, uint pos = 0)
        {
            if (Generic.IsNUBLooped)
            {
                if (bufferloop[0] == 0 || bufferloop[1] == 0) {
                    if (IsOpenMulti)
                    {
                        MultipleFilesLoopOKFlags[pos] = false;
                        MultipleLoopStarts[pos] = 0;
                        MultipleLoopEnds[pos] = 0;
                    }
                    else
                    {
                        MultipleFilesLoopOKFlags[0] = false;
                        MultipleLoopStarts[0] = 0;
                        MultipleLoopEnds[0] = 0;
                        LoopNG = true;
                    }
                    return; 
                }
                if (bufferloop[0] != 0 && bufferloop[1] != 0) {
                    if (IsOpenMulti)
                    {
                        MultipleFilesLoopOKFlags[pos] = true;
                        MultipleLoopStarts[pos] = bufferloop[0];
                        MultipleLoopEnds[pos] = bufferloop[1];
                    }
                    else
                    {
                        MultipleFilesLoopOKFlags[0] = true;
                        MultipleLoopStarts[0] = bufferloop[0];
                        MultipleLoopEnds[0] = bufferloop[1];
                    }
                }

                switch (samplerate)
                {
                    case 44100:
                        customTrackBar_LoopStart.Value = (int)Math.Round(bufferloop[0] / 44.1, MidpointRounding.AwayFromZero);//value[0];
                        numericUpDown_LoopStart.Value = customTrackBar_LoopStart.Value;
                        customTrackBar_LoopEnd.Value = (int)Math.Round(bufferloop[1] / 44.1, MidpointRounding.AwayFromZero);//value[1];
                        numericUpDown_LoopEnd.Value = customTrackBar_LoopEnd.Value;
                        break;
                    case 48000:
                        customTrackBar_LoopStart.Value = (int)Math.Round(bufferloop[0] / 48.0, MidpointRounding.AwayFromZero);//value[0];
                        numericUpDown_LoopStart.Value = customTrackBar_LoopStart.Value;
                        customTrackBar_LoopEnd.Value = (int)Math.Round(bufferloop[1] / 48.0, MidpointRounding.AwayFromZero);//value[1];
                        numericUpDown_LoopEnd.Value = customTrackBar_LoopEnd.Value;
                        break;
                    default:
                        break;
                }

                if (IsOpenMulti)
                {
                    if (MultipleFilesLoopOKFlags[pos] && !FormMain.checkBox_LoopEnable.Checked)
                    {
                        FormMain.checkBox_LoopEnable.Checked = true;
                        customTrackBar_LoopStart.Enabled = true;
                        customTrackBar_LoopStart.Invalidate();
                        customTrackBar_LoopEnd.Enabled = true;
                        customTrackBar_LoopEnd.Invalidate();
                        numericUpDown_LoopStart.Enabled = true;
                        numericUpDown_LoopEnd.Enabled = true;
                        button_LS_Current.Enabled = true;
                        button_LE_Current.Enabled = true;
                        button_SetEnd.Enabled = true;
                        button_SetStart.Enabled = true;
                        label_StartTweak.Enabled = true;
                        label_EndTweak.Enabled = true;
                    }
                    label_LoopStart.Text = "LoopStart: " + Generic.MultipleLoopStarts[pos].ToString() + " " + Localizable.Localization.SampleCaption;
                    label_LoopEnd.Text = "LoopEnd: " + Generic.MultipleLoopEnds[pos].ToString() + " " + Localizable.Localization.SampleCaption;
                    FormMain.textBox_LoopStart.Text = Generic.MultipleLoopStarts[pos].ToString();
                    FormMain.textBox_LoopEnd.Text = Generic.MultipleLoopEnds[pos].ToString();
                }
                else
                {
                    label_LoopStart.Text = "LoopStart: " + bufferloop[0].ToString() + " " + Localizable.Localization.SampleCaption;
                    label_LoopEnd.Text = "LoopEnd: " + bufferloop[1].ToString() + " " + Localizable.Localization.SampleCaption;
                    FormMain.textBox_LoopStart.Text = bufferloop[0].ToString();
                    FormMain.textBox_LoopEnd.Text = bufferloop[1].ToString();
                    LoopNG = false;
                }
            }
        }

        private void SetLoopPointWithMultipleFiles(int samplerate, uint pos)
        {
            if (!Generic.IsNUBLooped && IsOpenMulti && MultipleFilesLoopOKFlags[pos])
            {
                if (MultipleLoopStarts[pos] == 0 || MultipleLoopEnds[pos] == 0) { return; }

                switch (samplerate)
                {
                    case 44100:
                        customTrackBar_LoopStart.Value = (int)Math.Round(Generic.MultipleLoopStarts[pos] / 44.1, MidpointRounding.AwayFromZero);//value[0];
                        numericUpDown_LoopStart.Value = customTrackBar_LoopStart.Value;
                        customTrackBar_LoopEnd.Value = (int)Math.Round(Generic.MultipleLoopEnds[pos] / 44.1, MidpointRounding.AwayFromZero);//value[1];
                        numericUpDown_LoopEnd.Value = customTrackBar_LoopEnd.Value;
                        break;
                    case 48000:
                        customTrackBar_LoopStart.Value = (int)Math.Round(Generic.MultipleLoopStarts[pos] / 48.0, MidpointRounding.AwayFromZero);//value[0];
                        numericUpDown_LoopStart.Value = customTrackBar_LoopStart.Value;
                        customTrackBar_LoopEnd.Value = (int)Math.Round(Generic.MultipleLoopEnds[pos] / 48.0, MidpointRounding.AwayFromZero);//value[1];
                        numericUpDown_LoopEnd.Value = customTrackBar_LoopEnd.Value;
                        break;
                    default:
                        break;
                }

                if (MultipleFilesLoopOKFlags[pos] && !FormMain.checkBox_LoopEnable.Checked)
                {
                    FormMain.checkBox_LoopEnable.Checked = true;
                    customTrackBar_LoopStart.Enabled = true;
                    customTrackBar_LoopStart.Invalidate();
                    customTrackBar_LoopEnd.Enabled = true;
                    customTrackBar_LoopEnd.Invalidate();
                    numericUpDown_LoopStart.Enabled = true;
                    numericUpDown_LoopEnd.Enabled = true;
                    button_LS_Current.Enabled = true;
                    button_LE_Current.Enabled = true;
                    button_SetEnd.Enabled = true;
                    button_SetStart.Enabled = true;
                    label_StartTweak.Enabled = true;
                    label_EndTweak.Enabled = true;
                }
                label_LoopStart.Text = "LoopStart: " + Generic.MultipleLoopStarts[pos].ToString() + " " + Localizable.Localization.SampleCaption;
                label_LoopEnd.Text = "LoopEnd: " + Generic.MultipleLoopEnds[pos].ToString() + " " + Localizable.Localization.SampleCaption;
                FormMain.textBox_LoopStart.Text = Generic.MultipleLoopStarts[pos].ToString();
                FormMain.textBox_LoopEnd.Text = Generic.MultipleLoopEnds[pos].ToString();
            }
            else if (!Generic.IsNUBLooped && IsOpenMulti && !MultipleFilesLoopOKFlags[pos])
            {
                if (FormMain.checkBox_LoopEnable.Checked)
                {
                    FormMain.checkBox_LoopEnable.Checked = false;
                    customTrackBar_LoopStart.Enabled = false;
                    customTrackBar_LoopStart.Invalidate();
                    customTrackBar_LoopEnd.Enabled = false;
                    customTrackBar_LoopEnd.Invalidate();
                    numericUpDown_LoopStart.Enabled = false;
                    numericUpDown_LoopEnd.Enabled = false;
                    button_LS_Current.Enabled = false;
                    button_LE_Current.Enabled = false;
                    button_SetEnd.Enabled = false;
                    button_SetStart.Enabled = false;
                    label_StartTweak.Enabled = false;
                    label_EndTweak.Enabled = false;
                }
                label_LoopStart.Text = string.Empty;
                label_LoopEnd.Text = string.Empty;
                FormMain.textBox_LoopStart.Text = string.Empty;
                FormMain.textBox_LoopEnd.Text = string.Empty;
            }
        }

    }
}
